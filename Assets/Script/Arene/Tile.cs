using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;

    public List<Tile> adjacencyList = new List<Tile>();
    
    //Needed BFS (breadth first search)
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;
    
    //Needed A*
    public float f = 0;
    public float g = 0;
    public float h = 0;

    private void Update()
    {
        if(current)
        {
            GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if(target)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if(selectable)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void Reset()
    {
        walkable = true;
        current = false;
        target = false;
        selectable = false;

        adjacencyList.Clear();
    
        visited = false;
        parent = null;
        distance = 0;

        f = 0;
        g = 0;
        h = 0;
    }
    
    public void FindNeighbors(Tile target)
    {
        Reset();
        
        CheckTile(Vector3.forward, target);
        CheckTile(Vector3.back, target);
        CheckTile(Vector3.right, target);
        CheckTile(Vector3.left, target);
    }
    
    public void FindNeighborsAtk()
    {
        Reset();
        
        CheckTileAtk(Vector3.forward);
        CheckTileAtk(Vector3.back);
        CheckTileAtk(Vector3.right);
        CheckTileAtk(Vector3.left);
    }

    public void CheckTile(Vector3 dir, Tile target)
    {
        Vector3 halfExtents = new Vector3(.25f,.25f,.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + dir, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable == true)
            {
                RaycastHit hit;

                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || (tile == target))
                {
                    adjacencyList.Add(tile);
                }
            }
        }
    }
    
    public void CheckTileAtk(Vector3 dir)
    {
        Vector3 halfExtents = new Vector3(.25f,.25f,.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + dir, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null)
            {
                //RaycastHit hit;
                adjacencyList.Add(tile);
            }
        }
    }

    public GameObject GetGameObjectOnTop()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.up, out hit, 1);
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 1))
        {
            return hit.transform.gameObject;
        }
        return null;
    }
}