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
    private RoomEffect _roomEffect;
    
    private static bool _looted = false;
    [HideInInspector] public static bool ArtworkShown = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && ArtworkShown)
        {
            //todo effet
            DonjonManager.LaunchRoomEffect(_roomEffect);
            ResetValues();
        }
        
        if(_looted) return;
        switch (_roomType)
        {
            case RoomType.Boss:
                BossPanel.SetActive(true);
                _roomEffect = RoomEffect.Boss;
                ArtworkShown = true;
                break;
            case RoomType.Treasure:
                FouilleSelectCanvas.SetActive(true);
                break;
            case RoomType.Fighting:
                FightPanel.SetActive(true);
                _roomEffect = RoomEffect.Fight;
                ArtworkShown = true;
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

    public static void EnterRoomArtwork(RoomType roomType, bool looted)
    {
        _roomType = roomType;
        _looted = looted;
    }
    
    public void LootSelected()
    {
        switch (_roomType)
        {
            case RoomType.Normal:
                FouillePanel.SetActive(true);
                _roomEffect = RoomEffect.Loot;
                ArtworkShown = true;
                break;
            case RoomType.Treasure:
                TreasurePanel.SetActive(true);
                _roomEffect = RoomEffect.Treasure;
                ArtworkShown = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void RestSelected()
    {
        ReposPanel.SetActive(true);
        _roomEffect = RoomEffect.Rest;
        ArtworkShown = true;
    }

    public void ResetValues()
    {
        BossPanel.SetActive(false);
        FightPanel.SetActive(false);
        FouillePanel.SetActive(false);
        ReposPanel.SetActive(false);
        TreasurePanel.SetActive(false);
        FouilleSelectCanvas.SetActive(false);
        ArtworkShown = false;
    }
}