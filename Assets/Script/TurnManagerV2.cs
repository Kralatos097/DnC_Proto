using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurnManagerV2 : MonoBehaviour
{
    private static Queue<TacticsMovement> turnOrder = new Queue<TacticsMovement>();
    private static List<TacticsMovement> unitsList = new List<TacticsMovement>();

    private void Start()
    {
        Invoke(nameof(LateStart), 1);
    }
    
    private void LateStart()
    {
        StartCombat();
    }

    private void Update()
    {
        
    }

    static void StartCombat()
    {
        ListToQueue();
        
        Debug.Log(turnOrder.Count);
        
        StartTurn();
    }

    private static void EndCombat(bool pStatue)
    {
        //Victoire player
        if(pStatue)
        {
            //todo
        }
        //Défaite player
        else
        {
            //todo
        }
    }
    
    static void StartTurn()
    {
        if (ArePlayersAlive() && AreEnnemisAlive())
        {
            Debug.Log("Turn of : " + turnOrder.Peek().name);
            turnOrder.Peek().BeginTurn();
        }
        else
        {
            EndCombat(AreEnnemisAlive());
        }
    }

    public static void EndTurn()
    {
        TacticsMovement unit = turnOrder.Dequeue();
        unit.EndTurn();
        turnOrder.Enqueue(unit);
        StartTurn();
    }

    private static void ListToQueue()
    {
        while (unitsList.Count > 0)
        {
            int temp = -100;

            TacticsMovement unitRet = null;
            foreach (TacticsMovement unit in unitsList)
            {
                int init = unit.gameObject.GetComponent<CombatStat>().currInit;

                //si l'init est surerieur échange
                if (temp < init)
                {
                    temp = init;
                    unitRet = unit;
                }

                //si similaire on départage
                if (temp == init)
                {
                    int newBaseInit = unit.gameObject.GetComponent<CombatStat>().initiative;
                    int retBaseInit = unitRet.gameObject.GetComponent<CombatStat>().initiative;

                    //si l'init est surerieur échange
                    if (retBaseInit < newBaseInit)
                    {
                        temp = init;
                        unitRet = unit;
                    }
                    //si égale on départage
                    else if (retBaseInit == newBaseInit)
                    {
                        //on prioritise le player
                        if ((unit.gameObject.CompareTag("Player") && !unitRet.gameObject.CompareTag("Player"))
                            || (!unit.gameObject.CompareTag("Player") && unitRet.gameObject.CompareTag("Player")))
                        {
                            temp = init;
                            unitRet = unit;
                        }
                        //Si les 2 parties sont dans la meme équipe on choisie au hasard
                        else if (unit.gameObject.CompareTag("Player") == unitRet.gameObject.CompareTag("Player"))
                        {
                            int t = Random.Range(0, 2);
                            if (t == 0)
                            {
                                temp = init;
                                unitRet = unit;
                            }
                        }
                    }
                }
            }

            unitsList.Remove(unitRet);
            turnOrder.Enqueue(unitRet);
        }
    }
    
    public static void AddUnit(TacticsMovement unit)
    {
         unitsList.Add(unit);
    }
    
    //todo: remove of the list

    private static bool ArePlayersAlive()
    {
        foreach (TacticsMovement unit in turnOrder)
        {
            if(unit.gameObject.CompareTag("Player") && unit.gameObject.GetComponent<CombatStat>().isAlive)
            {
                return true;
            }
        }

        return false;
    }
    
    private static bool AreEnnemisAlive()
    {
        foreach (TacticsMovement unit in turnOrder)
        {
            if (unit.gameObject.CompareTag("Ennemi") && unit.gameObject.GetComponent<CombatStat>().isAlive)
            {
                return true;
            }
        }

        return false;
    }
}
