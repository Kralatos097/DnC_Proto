using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Consumable", order = 2)]
public class Consummable : Equipment
{
    private new void Start()
    {
        base.Start();
        stuffType = "Consumable";
    }
    
    public override void Effect()
    {
        throw new System.NotImplementedException();
    }

    public override void Effect(CombatStat cs)
    {
        throw new System.NotImplementedException();
    }
}
