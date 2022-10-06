using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatStat : MonoBehaviour
{
    public int initiative;

    public int MaxHp;
    private int _currHp;

    public int currHp
    {
        get => _currHp ;
        set
        {
            _currHp = value;
            if(_currHp <= 0)
            {
                _currHp = 0;
                isAlive = false;
                UnitDeath();
            }
            /*if(_currHp > 0)
            {
                isAlive = true;
            }*/
        }
    }

    [HideInInspector] public int currInit;

    [HideInInspector] public bool isAlive = true;

    //Todo
    /*public Equipment equipmentOne;
    public Equipment equipmentTwo;

    public Consummable consummable;
    
    public Passif passif;*/

    private void Start()
    {
        currHp = MaxHp;
    }

    public void RollInit()
    {
        currInit = initiative + Random.Range(1,7);
        
        Debug.Log(gameObject.name +" / curr: "+ currInit +" / init: " + initiative);
    }

    private void UnitDeath()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    [ContextMenu("KillUnit")]
    public void KillUnit()
    {
        currHp -= 100;
    }
}
