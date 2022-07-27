using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatStat : MonoBehaviour
{
    public int initiative;
    
    [HideInInspector] public int currInit;

    [HideInInspector] public bool isAlive = true;

    public void RollInit()
    {
        currInit = initiative + Random.Range(1,7);
        
        Debug.Log(gameObject.name +" / curr: "+ currInit +" / init: " + initiative);
    }
}
