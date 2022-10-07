using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DonjonTile : MonoBehaviour
{
    public bool FightingRoom = false;
    public bool TreasureRoom = false;
    public bool BossRoom = false;
    
    public bool Emptied = false;

    public string RoomScene;

    public SpriteRenderer _renderer;
    
    private bool _selectable;

    public bool Selectable
    {
        get => _selectable;

        set
        {
            _selectable = value;

            if(_selectable)
            {
                _renderer.color = new Color(1,0,0,1);
            }
            else
            {
                _renderer.color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void CheckAdjacentTiles()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, Vector3.forward, out hit, 1)) return;
        DonjonTile DjT = hit.collider.gameObject.GetComponent<DonjonTile>();
        if (DjT != null)
        {
            DjT.Selectable = true;
        }
        
        if (!Physics.Raycast(transform.position, Vector3.back, out hit, 1)) return;
        DjT = hit.collider.gameObject.GetComponent<DonjonTile>();
        if (DjT != null)
        {
            DjT.Selectable = true;
        }
        
        if (!Physics.Raycast(transform.position, Vector3.right, out hit, 1)) return;
        DjT = hit.collider.gameObject.GetComponent<DonjonTile>();
        if (DjT != null)
        {
            DjT.Selectable = true;
        }
        
        if (!Physics.Raycast(transform.position, Vector3.left, out hit, 1)) return;
        DjT = hit.collider.gameObject.GetComponent<DonjonTile>();
        if (DjT != null)
        {
            DjT.Selectable = true;
        }
    }
}
