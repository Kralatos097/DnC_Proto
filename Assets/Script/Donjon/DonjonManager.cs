using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.WSA;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class DonjonManager : MonoBehaviour
{
    [Header("Values")]
    public int restValue;

    public string bossScene;
    private static string _bossScene;

    [Header("Listes")]
    public List<string> fightingSceneList;
    private static List<string> _fightingSceneList;
    
    public List<Stuff> equipmentList;
    private static List<Stuff> _equipmentList;
    
    public List<Consummable> consoList;
    private static List<Consummable> _consoList;

    public static DonjonTile CurrentTile;

    [Header("CharaCard")]
    public PlayerBaseInfo warriorInfo;
    public PlayerBaseInfo thiefInfo;
    public PlayerBaseInfo clericInfo;
    public PlayerBaseInfo wizardInfo;

    private static bool _playerInfoPass = false;

    // Start is called before the first frame update
    void Start()
    {
        _fightingSceneList = fightingSceneList;
        _equipmentList = equipmentList;
        _consoList = consoList;
        _bossScene = bossScene;

        if(!_playerInfoPass)
        {
            AssignPlayerInfo();
        }
    }

    private void AssignPlayerInfo()
    {
        WarriorInfo.MaxHp = warriorInfo.MaxHp;
        WarriorInfo.CurrentHp = warriorInfo.MaxHp;
        WarriorInfo.Init = warriorInfo.Initiative;
        WarriorInfo.Movement = warriorInfo.Movement;
        WarriorInfo.EquipmentOne = warriorInfo.equipmentOne;
        WarriorInfo.EquipmentTwo = warriorInfo.equipmentTwo;
        WarriorInfo.Passif = warriorInfo.passif;
        WarriorInfo.Consumable = warriorInfo.consumable;
        
        ThiefInfo.MaxHp = thiefInfo.MaxHp;
        ThiefInfo.CurrentHp = thiefInfo.MaxHp;
        ThiefInfo.Init = thiefInfo.Initiative;
        ThiefInfo.Movement = thiefInfo.Movement;
        ThiefInfo.EquipmentOne = thiefInfo.equipmentOne;
        ThiefInfo.EquipmentTwo = thiefInfo.equipmentTwo;
        ThiefInfo.Passif = thiefInfo.passif;
        ThiefInfo.Consumable = thiefInfo.consumable;
        
        ClericInfo.MaxHp = clericInfo.MaxHp;
        ClericInfo.CurrentHp = clericInfo.MaxHp;
        ClericInfo.Init = clericInfo.Initiative;
        ClericInfo.Movement = clericInfo.Movement;
        ClericInfo.EquipmentOne = clericInfo.equipmentOne;
        ClericInfo.EquipmentTwo = clericInfo.equipmentTwo;
        ClericInfo.Passif = clericInfo.passif;
        ClericInfo.Consumable = clericInfo.consumable;
        
        WizardInfo.MaxHp = wizardInfo.MaxHp;
        WizardInfo.CurrentHp = wizardInfo.MaxHp;
        WizardInfo.Init = wizardInfo.Initiative;
        WizardInfo.Movement = wizardInfo.Movement;
        WizardInfo.EquipmentOne = wizardInfo.equipmentOne;
        WizardInfo.EquipmentTwo = wizardInfo.equipmentTwo;
        WizardInfo.Passif = wizardInfo.passif;
        WizardInfo.Consumable = wizardInfo.consumable;
    }

    public static void LaunchRoomEffect(RoomEffect roomEffect)
    {
        CurrentTile.emptied = true;
        switch(roomEffect)
        {
            case RoomEffect.Boss:
                LaunchBoss();
                break;
            case RoomEffect.Treasure:
                //todo
                break;
            case RoomEffect.Fight:
                LaunchFight();
                break;
            case RoomEffect.Rest:
                //todo
                break;
            case RoomEffect.Loot:
                LaunchLoot();
                break;
            case RoomEffect.Default:
            default:
                throw new ArgumentOutOfRangeException(nameof(roomEffect), roomEffect, null);
        }
    }

    //fight
    private static void LaunchFight()
    {
        string scene = SelectFightScene();
        
        Debug.Log(scene);
        _fightingSceneList.Remove(scene);
        Debug.Log(_fightingSceneList.Count); 
        SceneManager.LoadSceneAsync(scene);
    }

    private static string SelectFightScene()
    {
        int rand = Random.Range(0, _fightingSceneList.Count);
        string sceneName = _fightingSceneList[rand];
        
        return sceneName;
    }
    
    private static void LaunchBoss()
    {
        SceneManager.LoadSceneAsync(_bossScene);
    }
    
    //todo: loot
    private static void LaunchLoot()
    {
        Stuff stuff = PickStuff();
        UiManagerDj.StuffChoiceOne(stuff);

        /*int rand = Random.Range(0, 20);
        if(rand == 0)
        {
            //todo: Ambushed
        }
        else if (rand is > 0 and <= 3)
        {
            //todo: Trap
        }
        else if(rand is > 3 and <= 7)
        {
            Stuff stuff = PickStuff();
            UiManagerDj.StuffChoice(stuff);
        }
        else if (rand is > 7 and <= 13)
        {
            //todo: Consumable
            Consummable stuff = PickConsumable();
        }
        else
        {
            //todo: nothing
        }*/
    }

    private static Stuff PickStuff()
    {
        int ind = Random.Range(0, _equipmentList.Count);
        return _equipmentList[ind];
    }
    
    private static Consummable PickConsumable()
    {
        int ind = Random.Range(0, _consoList.Count);
        return _consoList[ind];
    }
    
    //todo: rest
    
    //todo: treasure
    private static void LaunchTreasure()
    {
        int rand = Random.Range(0, 5);
        if(rand == 0)
        {
            //todo: Consumable
            Consummable consumable = PickConsumable();
        }
        else
        {
            Stuff stuff = PickStuff();
            UiManagerDj.StuffChoiceOne(stuff);
        }
    }
}
