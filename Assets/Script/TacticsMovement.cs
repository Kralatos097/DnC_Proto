using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TacticsMovement : MonoBehaviour
{
    protected bool turn = false;
    
    private List<Tile> selectableTiles = new List<Tile>();
    private GameObject[] tiles;

    private Stack<Tile> path = new Stack<Tile>();
    private Tile currentTile;

    protected bool moving = false;
    protected bool attacking = false;
    [SerializeField] protected int move = 3;
    [SerializeField] protected float moveSpeed = 2;

    protected float MoveY = 1f;
    protected bool passM = false;

    private Vector3 velocity = new Vector3();
    private Vector3 heading = new Vector3();

    private float halfHeight = 0;

    [HideInInspector] public Tile actualTargetTile;

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;

        gameObject.GetComponent<CombatStat>().RollInit();
        
        TurnManagerV2.AddUnit(this);
    }
    
    protected void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    protected Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;
        
        if (Physics.Raycast(target.transform.position, Vector3.down, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    protected void ComputeAdjacencyList()
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(null);
        }
    }
    
    protected void ComputeAdjacencyList(Tile target)
    {
        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            t.FindNeighbors(target);
        }
    }

    protected void FindSelectableTile()
    {
        ComputeAdjacencyList();
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();
        
        process.Enqueue(currentTile);
        currentTile.visited = true;

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();
            
            selectableTiles.Add(t);
            t.selectable = true;

            if (t.distance < move)
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    protected void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.target = true;
        moving = true;

        Tile next = tile;
        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }

    protected void Move()
    {
        if (path.Count > 0)
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                CalculateHeading(target);
                SetHorizontalVelocity();

                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //tile center reached
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            RemoveSelectableTile();
            moving = false;
            
            //todo: fin du tour après les actions
            EndOfMovement();
        }
    }

    private void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    private void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    protected void RemoveSelectableTile()
    {
        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }
        
        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
        }
        
        selectableTiles.Clear();
    }

    public void BeginTurn()
    {
        turn = true;
    }

    public void EndTurn()
    {
        turn = false;
    }

    protected Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while(next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= move)
        {
            return t.parent;
        }

        Tile endTile = null;

        for (int i = 0; i <= move; i++)
        {
            endTile = tempPath.Pop();
        }

        return endTile;
    }

    protected void FindPath(Tile targetTile)
    {
        ComputeAdjacencyList(targetTile);
        GetCurrentTile();

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();
        
        openList.Add(currentTile);
        currentTile.h = Vector3.Distance(currentTile.transform.position, targetTile.transform.position);
        currentTile.f = currentTile.h;

        while (openList.Count > 0)
        {
            Tile t = FindLowestF(openList);
            
            closedList.Add(t);

            if (t == targetTile)
            {
                actualTargetTile = FindEndTile(t);
                MoveToTile(actualTargetTile);
                return;
            }

            foreach (Tile tile in t.adjacencyList)
            {
                if (closedList.Contains(tile))
                {
                    //Do nothing, already processed
                }
                else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if (tempG < tile.g)
                    {
                        tile.parent = t;
                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                else
                {
                    tile.parent = t;

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, targetTile.transform.position);

                    tile.f = tile.g + tile.h;
                    
                    openList.Add(tile);
                }
            }
        }
        
        //todo - what do you do if there is no path to the target tile?
        Debug.Log("Path not Found");
    }

    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach (Tile t in list)
        {
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }

        list.Remove(lowest);
        
        return lowest;
    }

    protected virtual void EndOfMovement()
    {
        //Fin du Soulevement du pion lors du mouvement
        transform.GetChild(0).Translate(0,-MoveY,0);
        passM = false;
    }

    protected void Attack()
    {
        
    }
}
