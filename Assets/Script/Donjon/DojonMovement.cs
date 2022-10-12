using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DojonMovement : MonoBehaviour
{
    public float speed;

    private DonjonTile currTile;
    private Vector3 target;

    private void Start()
    {
        target = transform.position;
        Invoke("GetCurrentTile",.1f);
    }

    private void Update()
    {
        CheckMove();
        
        MoveToTile(target);
    }

    public void GetCurrentTile()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 1);
        DonjonTile djTile =  hit.collider.transform.gameObject.GetComponent<DonjonTile>();
        if(djTile != null)
        {
            currTile = djTile;
            djTile.current = true;
        }
    }

    private void CheckMove()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay((Input.mousePosition));

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    DonjonTile t = hit.collider.transform.GetComponent<DonjonTile>();

                    if (t.selectable)
                    {
                        //Todo move to tile
                        currTile.current = false;
                        target = new Vector3(t.transform.position.x, transform.position.y, t.transform.position.z);
                        t.current = true;
                        currTile = t;
                    }
                }
            }
        }
    }

    private void MoveToTile(Vector3 target)
    {
        transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
    }
}
