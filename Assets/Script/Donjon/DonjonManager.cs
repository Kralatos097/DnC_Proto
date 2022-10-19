using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class DonjonManager : MonoBehaviour
{
    [Header("Values")]
    public int restValue;
    
    [Header("Listes")]
    public List<string> fightingSceneList;
    private static List<string> _fightingSceneList;
    
    public List<Stuff> equipmentList;
    private static List<Stuff> _equipmentList;
    
    public List<Consummable> consoList;
    private static List<Consummable> _consoList;

    // Start is called before the first frame update
    void Start()
    {
        _fightingSceneList = fightingSceneList;
        _equipmentList = equipmentList;
        _consoList = consoList;
    }

    public static void LaunchRoomEffect(RoomEffect roomEffect)
    {
        switch(roomEffect)
        {
            case RoomEffect.Boss:
                break;
            case RoomEffect.Treasure:
                break;
            case RoomEffect.Fight:
                LaunchFight();
                break;
            case RoomEffect.Rest:
                break;
            case RoomEffect.Loot:
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
    
    //todo: loot
    
    //todo: rest
    
    //todo: boss
    
    //todo: treasure
}
