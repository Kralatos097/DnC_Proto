using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Equipment", order = 1)]
public class Equipment : Stuff
{
    public int Damage;
    public int CD;
    public int Range;
    
    public override void Effect()
    {
        
    }

    public override void Effect(CombatStat cs)
    {
        cs.currHp -= Damage;
    }
}
