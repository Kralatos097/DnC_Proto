using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UnitCard", order = 2)]
public class PlayerBaseInfo : ScriptableObject
{
    public int MaxHp;
    public int Initiative;
    public int Movement;
    public Equipment equipmentOne;
    public Equipment equipmentTwo;
    public Passif passif;
    public Consummable consumable;
}
