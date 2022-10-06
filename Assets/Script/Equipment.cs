using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Equipement", order = 1)]
public class Equipment : Stuff
{
    public int Damage;
    public int CD;

    public override void Effect()
    {
        switch (equipType)
        {
            case EquipType.Default:
                break;
            case EquipType.Self:
                break;
            case EquipType.Cac:
                CacAtk();
                break;
            case EquipType.Distance:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        throw new System.NotImplementedException();
    }

    private void CacAtk()
    {
        
    }
}
