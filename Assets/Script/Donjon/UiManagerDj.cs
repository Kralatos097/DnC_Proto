using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManagerDj : MonoBehaviour
{
    [SerializeField] private GameObject BossPanel;
    [SerializeField] private GameObject FouillePanel;
    [SerializeField] private GameObject TreasurePanel;
    [SerializeField] private GameObject FightPanel;
    [SerializeField] private GameObject ReposPanel;

    [SerializeField] private GameObject FouilleSelectCanvas;

    private static RoomType _roomType = RoomType.Starting;
    
    private bool _looted = false;

    private void Update()
    {
        if(_looted) return;
        switch (_roomType)
        {
            case RoomType.Boss:
                BossPanel.SetActive(true);
                break;
            case RoomType.Treasure:
                FouilleSelectCanvas.SetActive(true);
                break;
            case RoomType.Fighting:
                FightPanel.SetActive(true);
                break;
            case RoomType.Normal:
                FouilleSelectCanvas.SetActive(true);
                break;
            
            case RoomType.Starting:
            default:
                ResetValues();
                break;
        }
    }

    public static void EnterRoomArtwork(RoomType roomType)
    {
        _roomType = roomType;
    }
    
    public void LootSelected()
    {
        switch (_roomType)
        {
            case RoomType.Normal:
                FouillePanel.SetActive(true);
                break;
            case RoomType.Treasure:
                TreasurePanel.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        _looted = true;
    }

    public void RestSelected()
    {
        ReposPanel.SetActive(true);
    }

    public void ResetValues()
    {
        BossPanel.SetActive(false);
        FightPanel.SetActive(false);
        FouillePanel.SetActive(false);
        ReposPanel.SetActive(false);
        TreasurePanel.SetActive(false);
        FouilleSelectCanvas.SetActive(false);
    }
}