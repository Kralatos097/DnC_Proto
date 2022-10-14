using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DojonMovement : MonoBehaviour
{
    public float speed;

    private DonjonTile currTile;
    private Vector3 target;
    private bool _canMove = false;

    private void Start()
    {
        target = transform.position;
        GetCurrentTile();
        _canMove = true;
    }

    private void Update()
    {
        if(_canMove)
            CheckMove();
        if (Vector3.Distance(transform.position, target) >= .02f)
        {
            UiManagerDj.EnterRoomArtwork(RoomType.Starting);
            _canMove = false;
        }
        else
        {
            if(currTile.emptied) return;
            
            //Todo: Lancer l'effet de la piece
            //GetCurrentTile();
            UiManagerDj.EnterRoomArtwork(currTile.roomType);
            switch (currTile.roomType)
            {
                case RoomType.Normal:
                    //todo: fouille
                    break;
                case RoomType.Boss:
                    //todo: lance combat
                    break;
                case RoomType.Treasure:
                    //todo: fouille - rates speciaux
                    break;
                case RoomType.Fighting:
                    //todo: lance combat
                    break;
                case RoomType.Starting:
                default:
                    break;
            }
            _canMove = true;
        }
        if(transform.position != target)
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
