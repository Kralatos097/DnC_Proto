using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : TacticsMovement
{
    private GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(!turn) return;
        
        if (!moving)
        {
            //Début du Soulevement du pion lors du mouvement
            if(!passM)
            {
                transform.GetChild(0).Translate(0, MoveY, 0);
                passM = true;
            }
            
            FindNearestTarget();
            CalculatePath();
            FindSelectableTile();
            actualTargetTile.target = true;
        }
        else
        {
            Move();
        }
    }

    private void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject obj in targets)
        {
            float d = Vector3.Distance(transform.position, obj.transform.position);

            if (d < distance)
            {
                distance = d;
                nearest = obj;
            }
        }
        
        target = nearest; 
    }

    private void CalculatePath()
    {
        Tile targetTile = GetTargetTile(target);
        FindPath(targetTile);
    }

    protected override void EndOfMovement()
    {
        base.EndOfMovement();
        TurnManagerV2.EndTurn();
    }
    
    protected override void EndOfAttack()
    {
        TurnManagerV2.EndTurn();
    }
}
