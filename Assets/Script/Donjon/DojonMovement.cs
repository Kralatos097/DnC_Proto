using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DojonMovement : MonoBehaviour
{
    public float speed;

    private Vector3 target;
    private bool _canMove = false;

    private void Start()
    {
        target = transform.position;
        DonjonManager.CurrentTile = GetCurrentTile();
        _canMove = true;
    }

    private void Update()
    {
        if(_canMove && !UiManagerDj.ArtworkShown && !UiManagerDj.InChoice)
            CheckMove();
        if (Vector3.Distance(transform.position, target) >= .02f)
        {
            UiManagerDj.EnterRoomArtwork(RoomType.Starting);
            _canMove = false;
        }
        else
        {
            if(!DonjonManager.CurrentTile.emptied) 
                UiManagerDj.EnterRoomArtwork(DonjonManager.CurrentTile.roomType);
            _canMove = true;
        }
        if(transform.position != target)
            MoveToTile(target);
    }

    public DonjonTile GetCurrentTile()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 1);
        DonjonTile djTile =  hit.collider.transform.gameObject.GetComponent<DonjonTile>();
        if(djTile != null)
        {
            djTile.current = true;
            return djTile;
        }
        else
        {
            return null;
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
                        DonjonManager.CurrentTile.current = false;
                        target = new Vector3(t.transform.position.x, transform.position.y, t.transform.position.z);
                        t.current = true;
                        DonjonManager.CurrentTile = t;
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
