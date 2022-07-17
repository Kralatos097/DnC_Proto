using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    static Dictionary<string, List<TacticsMovement>> units = new Dictionary<string, List<TacticsMovement>>();
    static Queue<string> turnKey = new Queue<string>();
    static Queue<TacticsMovement> turnTeam = new Queue<TacticsMovement>();

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log("Key " + turnKey.Count);
        Debug.Log("Team " + turnTeam.Count);
        Debug.Log("Dico " + units.Count);
        Debug.Log("------------------------");*/
        if (turnTeam.Count == 0)
        {
            InitTeamTurnQueue();
        }
    }

    static void InitTeamTurnQueue()
    {
        List<TacticsMovement> teamList = units[turnKey.Peek()];

        foreach (TacticsMovement unit in teamList)
        {
            turnTeam.Enqueue(unit);
        }

        StartTurn();
    }

    static void StartTurn()
    {
        if (turnTeam.Count > 0)
        {
            turnTeam.Peek().BeginTurn();
        }
    }
    
    public static void EndTurn()
    {
        TacticsMovement unit = turnTeam.Dequeue();
        unit.EndTurn();

        if (turnTeam.Count > 0)
        {
            StartTurn();
        }
        else
        {
            string team = turnKey.Dequeue();
            turnKey.Enqueue(team);
            InitTeamTurnQueue();
        }
    }
    
    public static void AddUnit(TacticsMovement unit)
    {
        List<TacticsMovement> list;

        if (!units.ContainsKey(unit.tag))
        {
            list = new List<TacticsMovement>();
            units[unit.tag] = list;

            if (!turnKey.Contains(unit.tag))
            {
                turnKey.Enqueue(unit.tag);
            }
        }
        else
        {
            list = units[unit.tag];
        }

        list.Add(unit);
    }
    
    //todo: remove of the list
}
