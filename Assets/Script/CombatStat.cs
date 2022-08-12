using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatStat : MonoBehaviour
{
    public int initiative;

    public int MaxHp;
    private int _currHp;
    
    [HideInInspector] public int currInit;

    [HideInInspector] public bool isAlive = true;

    private void Start()
    {
        _currHp = MaxHp;
    }

    public void RollInit()
    {
        currInit = initiative + Random.Range(1,7);
        
        Debug.Log(gameObject.name +" / curr: "+ currInit +" / init: " + initiative);
    }
}
