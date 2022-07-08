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
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    public void FindNeighbors()
    {
        Reset();
        
        CheckTile(Vector3.forward);
        CheckTile(Vector3.back);
        CheckTile(Vector3.right);
        CheckTile(Vector3.left);
    }

    public void CheckTile(Vector3 dir)
    {
        Vector3 halfExtents = new Vector3(.25f,.25f,.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + dir, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable == true)
            {
                RaycastHit hit;

                if (Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1))
                {
                    adjacencyList.Add(tile);
                }
            }
        }
    }
}
