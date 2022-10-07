using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DojonMovement : MonoBehaviour
{
    private void Start()
    {
        CheckMove();
    }

    public void CheckMove()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, 1)) return;
        DonjonTile DjT = hit.collider.gameObject.GetComponent<DonjonTile>();
        if (DjT != null)
        {
            DjT.CheckAdjacentTiles();
        }
    }
}
