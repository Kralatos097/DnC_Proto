using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManagerDj : MonoBehaviour
{
    [SerializeField] private GameObject BossPanel;
    [SerializeField] private GameObject FouillePanel;
    [SerializeField] private GameObject TreasurePanel;
    [SerializeField] private GameObject FightPanel;
    [SerializeField] private GameObject ReposPanel;

    [SerializeField] private GameObject FouilleSelectCanvas;
    
    [SerializeField] private GameObject StuffCharaSelectPanel;
    [SerializeField] private GameObject StuffReplaceSelectPanel;
    [SerializeField] private GameObject StuffIconPanel;

    public static Action<Stuff> StuffChoiceOne;
    public static Action<Stuff> StuffChoiceTwo;

    private static RoomType _roomType = RoomType.Starting;
    private RoomEffect _roomEffect;

    private Stuff newStuff;
    
    [HideInInspector] public static bool ArtworkShown = false;

    private bool _pass = true;

    private void Start()
    {
        StuffChoiceOne = StuffChoice;
        //StuffChoiceTwo = ;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && ArtworkShown)
        {
            //todo effet
            DonjonManager.LaunchRoomEffect(_roomEffect);
            ResetValues();
        }

        if(DonjonManager.CurrentTile.emptied) return;
        if(!_pass)
        {
            FouilleSelectCanvas.SetActive(false);
            return;
        }
        switch (_roomType)
        {
            case RoomType.Boss:
                BossPanel.SetActive(true);
                _roomEffect = RoomEffect.Boss;
                ArtworkShown = true;
                _pass = false;
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

    public static void EnterRoomArtwork(RoomType roomType)
    {
        _roomType = roomType;
    }
    
    public void LootSelected()
    {
        _pass = false;
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
        _pass = false;
        ReposPanel.SetActive(true);
        _roomEffect = RoomEffect.Rest;
        ArtworkShown = true;
    }

    private void ResetValues()
    {
        BossPanel.SetActive(false);
        FightPanel.SetActive(false);
        FouillePanel.SetActive(false);
        ReposPanel.SetActive(false);
        TreasurePanel.SetActive(false);
        FouilleSelectCanvas.SetActive(false);
        ArtworkShown = false;
        _pass = true;
    }

    private void StuffChoice(Stuff stuff)
    {
        StuffCharaSelectPanel.SetActive(true);
        newStuff = stuff;
    }

    public void CharaSelect(int nb)
    {
        switch(nb)
        {
            case 0: //Warrior
                //todo: recup l'equipement du perso et le mettre dans l'UI de selection d'equipement puis l'afficher
                StuffIconPanel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = newStuff.Logo;
                EquipChoiceIconChange(WarriorInfo.Equipment1, StuffIconPanel.transform.GetChild(1).gameObject);
                EquipChoiceIconChange(WarriorInfo.Equipment2, StuffIconPanel.transform.GetChild(2).gameObject);
                EquipChoiceIconChange(WarriorInfo.Passif, StuffIconPanel.transform.GetChild(3).gameObject);
                EquipChoiceIconChange(WarriorInfo.Consumable, StuffIconPanel.transform.GetChild(4).gameObject);
                break;
            
            case 1: //Thief
                break;
            
            case 2: //Cleric
                break;
            
            case 3: //Wizard
                break;
            
            default:
                break;
        }
        StuffCharaSelectPanel.SetActive(false);
        StuffReplaceSelectPanel.SetActive(true);
    }

    private void EquipChoiceIconChange(Stuff stuff, GameObject Go)
    {
        if (stuff != null)
        {
            Go.GetComponent<Image>().sprite = stuff.Logo;
            Go.GetComponent<Button>().interactable = true;
        }
        else
        {
            Go.GetComponent<Image>().sprite = null;
            Go.GetComponent<Button>().interactable = false;
        }
    }
}