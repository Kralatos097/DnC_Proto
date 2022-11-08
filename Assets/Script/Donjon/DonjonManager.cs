using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class DonjonManager : MonoBehaviour
{
    [Header("Values")]
    public int restValue;

    public string bossScene;
    private static string _bossScene;

    public static GameObject _gameContainer;

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

    private static string DungeonSceneName;

    private void Awake()
    {
        DungeonSceneName = SceneManager.GetActiveScene().name;
    }

    // Start is called before the first frame update
    void Start()
    {
        _fightingSceneList = fightingSceneList;
        _equipmentList = equipmentList;
        _consoList = consoList;
        _bossScene = bossScene;
        _gameContainer = GameObject.Find("GameContainer");

        if(!_playerInfoPass)
        {
            AssignPlayerInfo();
            _playerInfoPass = true;
        }
        
        UiManagerDj.playerInfoUi();
    }

    private void Update()
    {
        UiManagerDj.playerInfoUi();
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
                LaunchTreasure();
                break;
            case RoomEffect.Fight:
                LaunchFight();
                break;
            case RoomEffect.Rest:
                //todo
                LaunchRest();
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
        _gameContainer.SetActive(false);
        AsyncOperation op = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        op.completed += operation =>
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
        };
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
        /*Stuff stuff = PickStuff();
        UiManagerDj.StuffChoiceOne(stuff);*/

        int rand = Random.Range(0, 20);
        Debug.Log(rand);
        if(rand == 0)
        {
            //todo: Ambushed
            UiManagerDj.InChoice = false;
            LaunchFight();
        }
        else if (rand is > 0 and <= 3)
        {
            //todo: Trap
            LaunchTrap();
            UiManagerDj.InChoice = false;
        }
        else if(rand is > 3 and <= 7)
        {
            Stuff stuff = PickStuff();
            UiManagerDj.StuffChoiceOne(stuff);
        }
        else if (rand is > 7 and <= 13)
        {
            //todo: consumable -> check needed
            Consummable stuff = PickConsumable();
            UiManagerDj.StuffChoiceOne(stuff);
        }
        else
        {
            //todo: nothing
            UiManagerDj.InChoice = false;
        }
        UiManagerDj.playerInfoUi();
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
    private static void LaunchRest()
    {
        WarriorInfo.CurrentHp += Random.Range(1, 7);
        ThiefInfo.CurrentHp += Random.Range(1, 7);
        ClericInfo.CurrentHp += Random.Range(1, 7);
        WizardInfo.CurrentHp += Random.Range(1, 7);
        
        Debug.Log(WarriorInfo.CurrentHp);
        Debug.Log(ThiefInfo.CurrentHp);
        Debug.Log(ClericInfo.CurrentHp);
        Debug.Log(WizardInfo.CurrentHp);
    }
    
    //todo: treasure
    private static void LaunchTreasure()
    {
        int rand = Random.Range(0, 5);
        if(rand == 0)
        {
            Stuff stuff = PickConsumable();
            UiManagerDj.StuffChoiceOne(stuff);
        }
        else
        {
            Stuff stuff = PickStuff();
            UiManagerDj.StuffChoiceOne(stuff);
        }
        UiManagerDj.playerInfoUi();
    }
    
    private static void LaunchTrap()
    {
        int comp = WarriorInfo.CurrentHp;
        int minus = Random.Range(1, 4);
        if (comp - minus > 0)
        {
            WarriorInfo.CurrentHp -= minus;
        }
        else
            WarriorInfo.CurrentHp = 1;
        
        comp = ThiefInfo.CurrentHp;
        minus = Random.Range(1, 4);
        if (comp - minus > 0)
        {
            ThiefInfo.CurrentHp -= minus;
        }
        else
            ThiefInfo.CurrentHp = 1;
        
        comp = ClericInfo.CurrentHp;
        minus = Random.Range(1, 4);
        if (comp - minus > 0)
        {
            ClericInfo.CurrentHp -= minus;
        }
        else
            ClericInfo.CurrentHp = 1;
        
        comp = WizardInfo.CurrentHp;
        minus = Random.Range(1, 4);
        if (comp - minus > 0)
        {
            WizardInfo.CurrentHp -= minus;
        }
        else
            WizardInfo.CurrentHp = 1;
        
        Debug.Log(WarriorInfo.CurrentHp);
        Debug.Log(ThiefInfo.CurrentHp);
        Debug.Log(ClericInfo.CurrentHp);
        Debug.Log(WizardInfo.CurrentHp);
    }

    public static string GetDungeonSceneName()
    {
        return DungeonSceneName;
    }
}
