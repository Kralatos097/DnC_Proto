using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetDamageItem : Equipment
{
    public override void Effect()
    {
        throw new NotImplementedException();
    }

    public override void Effect(CombatStat cs)
    {
        cs.currHp -= Damage;
        
    }
}