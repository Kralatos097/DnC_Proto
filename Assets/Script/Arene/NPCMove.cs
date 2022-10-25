using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : TacticsMovement
{
    private GameObject target;
    private bool _alreadyMoved = false;
    
    [SerializeField] protected PlayerBaseInfo UnitInfo;
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    protected override void GetUnitInfo()
    {
        CombatStat _combatStat = gameObject.GetComponent<CombatStat>();
        
        _combatStat.MaxHp = UnitInfo.MaxHp;
        _combatStat.currHp = UnitInfo.MaxHp;
        _combatStat.initiative = UnitInfo.Initiative;

        move = UnitInfo.Movement;
        equipmentOne = UnitInfo.equipmentOne;
        equipmentTwo = UnitInfo.equipmentTwo;
        passif = UnitInfo.passif;
        consummable = UnitInfo.consumable;
    }

    // Update is called once per frame
    void Update()
    {
        if(!turn) return;
        PlayersTurn = false;

        atkRange = equipmentOne.Range;
        bool canAtk = true;

        GameObject temp = AlliesInAttackRange();
        RemoveSelectableTile();
        if (temp != null && !moving)
        {
            Debug.Log("Ennemi Atk !");
            attacking = true;
            Attack(temp.GetComponent<CombatStat>(), 1);
            return;
        }
        else
        {
            canAtk = false;
        }
        if(!_alreadyMoved && !attacking)
        {
            if (!moving)
            {
                //Début du Soulevement du pion lors du mouvement
                if (!passM)
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
                canAtk = true;
            }
        }

        if (_alreadyMoved && !canAtk)
        {
            EndTurnT();
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
        _alreadyMoved = true;
        base.EndOfMovement();
    }
    
    protected override void EndOfAttack()
    {
        base.EndOfAttack();
        EndTurnT();
    }

    protected void EndTurnT()
    {
        TurnManagerV2.EndTurn();
        attacking = false;
        _alreadyMoved = false;
    }
}
