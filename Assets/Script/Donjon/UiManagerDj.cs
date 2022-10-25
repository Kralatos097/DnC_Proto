using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
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

    private static RoomType _roomType = RoomType.Starting;
    private RoomEffect _roomEffect;

    private Stuff _newStuff;
    private Perso _charaSelected = Perso.Default;
    
    public static bool ArtworkShown = false;
    public static bool InChoice = false;

    private bool _pass = true;

    public Sprite EmptyIcon;

    private void Start()
    {
        ArtworkShown = false;
        StuffChoiceOne = StuffChoice;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && ArtworkShown)
        {
            //todo effet
            DonjonManager.LaunchRoomEffect(_roomEffect);
            ResetArtworks();
        }
        if (Input.GetMouseButtonDown(1) && StuffReplaceSelectPanel.activeSelf)
        {
            ReturnChange();
        }

        if(DonjonManager.CurrentTile.emptied)
        {
            ResetArtworks();
            return;
        }
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
                ResetArtworks();
                break;
        }
    }

    public static void EnterRoomArtwork(RoomType roomType)
    {
        _roomType = roomType;
    }
    
    public void LootSelected()
    {
        InChoice = true;
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

    private void ResetArtworks()
    {
        BossPanel.SetActive(false);
        FightPanel.SetActive(false);
        FouillePanel.SetActive(false);
        ReposPanel.SetActive(false);
        TreasurePanel.SetActive(false);
        FouilleSelectCanvas.SetActive(false);
        _pass = true;
        ArtworkShown = false;
    }

    private void StuffChoice(Stuff stuff)
    {
        StuffCharaSelectPanel.SetActive(true);
        _newStuff = stuff;
    }

    public void CharaSelect(int nb)
    {
        StuffIconPanel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = _newStuff.Logo;
        ChoiceButtonInteract();
        switch(nb)
        {
            case 0: //Warrior
                _charaSelected = Perso.Warrior;
                //todo: recup l'equipement du perso et le mettre dans l'UI de selection d'equipement puis l'afficher
                EquipChoiceIconChange(WarriorInfo.EquipmentOne, StuffIconPanel.transform.GetChild(1).gameObject);
                EquipChoiceIconChange(WarriorInfo.EquipmentTwo, StuffIconPanel.transform.GetChild(2).gameObject);
                EquipChoiceIconChange(WarriorInfo.Passif, StuffIconPanel.transform.GetChild(3).gameObject);
                EquipChoiceIconChange(WarriorInfo.Consumable, StuffIconPanel.transform.GetChild(4).gameObject);
                break;
            
            case 1: //Thief
                _charaSelected = Perso.Thief;
                EquipChoiceIconChange(ThiefInfo.EquipmentOne, StuffIconPanel.transform.GetChild(1).gameObject);
                EquipChoiceIconChange(ThiefInfo.EquipmentTwo, StuffIconPanel.transform.GetChild(2).gameObject);
                EquipChoiceIconChange(ThiefInfo.Passif, StuffIconPanel.transform.GetChild(3).gameObject);
                EquipChoiceIconChange(ThiefInfo.Consumable, StuffIconPanel.transform.GetChild(4).gameObject);
                break;
            
            case 2: //Cleric
                _charaSelected = Perso.Cleric;
                EquipChoiceIconChange(ClericInfo.EquipmentOne, StuffIconPanel.transform.GetChild(1).gameObject);
                EquipChoiceIconChange(ClericInfo.EquipmentTwo, StuffIconPanel.transform.GetChild(2).gameObject);
                EquipChoiceIconChange(ClericInfo.Passif, StuffIconPanel.transform.GetChild(3).gameObject);
                EquipChoiceIconChange(ClericInfo.Consumable, StuffIconPanel.transform.GetChild(4).gameObject);
                break;
            
            case 3: //Wizard
                _charaSelected = Perso.Wizard;
                EquipChoiceIconChange(WizardInfo.EquipmentOne, StuffIconPanel.transform.GetChild(1).gameObject);
                EquipChoiceIconChange(WizardInfo.EquipmentTwo, StuffIconPanel.transform.GetChild(2).gameObject);
                EquipChoiceIconChange(WizardInfo.Passif, StuffIconPanel.transform.GetChild(3).gameObject);
                EquipChoiceIconChange(WizardInfo.Consumable, StuffIconPanel.transform.GetChild(4).gameObject);
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
        }
        else
        {
            Go.GetComponent<Image>().sprite = null;
        }
    }

    private void ChoiceButtonInteract()
    {
        if(_newStuff.GetType() == typeof(Consummable))
        {
            StuffIconPanel.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            StuffIconPanel.transform.GetChild(2).gameObject.GetComponent<Button>().interactable = false;
            StuffIconPanel.transform.GetChild(3).gameObject.GetComponent<Button>().interactable = false;
            StuffIconPanel.transform.GetChild(4).gameObject.GetComponent<Button>().interactable = true;
        }
        else if(_newStuff.GetType() == typeof(Passif))
        {
            StuffIconPanel.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            StuffIconPanel.transform.GetChild(2).gameObject.GetComponent<Button>().interactable = false;
            StuffIconPanel.transform.GetChild(3).gameObject.GetComponent<Button>().interactable = true;
            StuffIconPanel.transform.GetChild(4).gameObject.GetComponent<Button>().interactable = false;
        }
        else if(_newStuff.GetType() == typeof(Equipment))
        {
            StuffIconPanel.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = true;
            StuffIconPanel.transform.GetChild(2).gameObject.GetComponent<Button>().interactable = true;
            StuffIconPanel.transform.GetChild(3).gameObject.GetComponent<Button>().interactable = false;
            StuffIconPanel.transform.GetChild(4).gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void ChangeEquipOneButton()
    {
        Stuff stuff = null;
        switch (_charaSelected)
        {
            case Perso.Warrior:
                stuff = WarriorInfo.EquipmentOne;
                WarriorInfo.EquipmentOne = (Equipment)_newStuff;
                break;
            case Perso.Thief:
                stuff = ThiefInfo.EquipmentOne;
                ThiefInfo.EquipmentOne = (Equipment)_newStuff;
                break;
            case Perso.Cleric:
                stuff = ClericInfo.EquipmentOne;
                ClericInfo.EquipmentOne = (Equipment)_newStuff;
                break;
            case Perso.Wizard:
                stuff = WizardInfo.EquipmentOne;
                WizardInfo.EquipmentOne = (Equipment)_newStuff;
                break;
            case Perso.Default:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        GameObject buttonClicked = EventSystem.current.currentSelectedGameObject;
        
        buttonClicked.GetComponent<Button>().image.sprite = _newStuff.Logo;
        StuffIconPanel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = stuff != null ? stuff.Logo : EmptyIcon;

        _newStuff = stuff;
    }
    
    public void ChangeEquipTwoButton()
    {
        Stuff stuff = null;
        switch (_charaSelected)
        {
            case Perso.Warrior:
                stuff = WarriorInfo.EquipmentTwo;
                WarriorInfo.EquipmentTwo = (Equipment)_newStuff;
                break;
            case Perso.Thief:
                stuff = ThiefInfo.EquipmentTwo;
                ThiefInfo.EquipmentTwo = (Equipment)_newStuff;
                break;
            case Perso.Cleric:
                stuff = ClericInfo.EquipmentTwo;
                ClericInfo.EquipmentTwo = (Equipment)_newStuff;
                break;
            case Perso.Wizard:
                stuff = WizardInfo.EquipmentTwo;
                WizardInfo.EquipmentTwo = (Equipment)_newStuff;
                break;
            case Perso.Default:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        GameObject buttonClicked = EventSystem.current.currentSelectedGameObject;
        
        buttonClicked.GetComponent<Button>().image.sprite = _newStuff.Logo;
        StuffIconPanel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = stuff != null ? stuff.Logo : EmptyIcon;

        _newStuff = stuff;
    }
    
    public void ChangePassifButton()
    {
        Stuff stuff = null;
        switch (_charaSelected)
        {
            case Perso.Warrior:
                stuff = WarriorInfo.Passif;
                WarriorInfo.Passif = (Passif)_newStuff;
                break;
            case Perso.Thief:
                stuff = ThiefInfo.Passif;
                ThiefInfo.Passif = (Passif)_newStuff;
                break;
            case Perso.Cleric:
                stuff = ClericInfo.Passif;
                ClericInfo.Passif = (Passif)_newStuff;
                break;
            case Perso.Wizard:
                stuff = WizardInfo.Passif;
                WizardInfo.Passif = (Passif)_newStuff;
                break;
            case Perso.Default:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        GameObject buttonClicked = EventSystem.current.currentSelectedGameObject;
        
        buttonClicked.GetComponent<Button>().image.sprite = _newStuff.Logo;
        StuffIconPanel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = stuff != null ? stuff.Logo : EmptyIcon;

        _newStuff = stuff;
    }
    
    public void ChangeConsumableButton()
    {
        Stuff stuff = null;
        switch (_charaSelected)
        {
            case Perso.Warrior:
                stuff = WarriorInfo.Consumable;
                WarriorInfo.Consumable = (Consummable)_newStuff;
                break;
            case Perso.Thief:
                stuff = ThiefInfo.Consumable;
                ThiefInfo.Consumable = (Consummable)_newStuff;
                break;
            case Perso.Cleric:
                stuff = ClericInfo.Consumable;
                ClericInfo.Consumable = (Consummable)_newStuff;
                break;
            case Perso.Wizard:
                stuff = WizardInfo.Consumable;
                WizardInfo.Consumable = (Consummable)_newStuff;
                break;
            case Perso.Default:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        GameObject buttonClicked = EventSystem.current.currentSelectedGameObject;
        
        buttonClicked.GetComponent<Button>().image.sprite = _newStuff.Logo;
        StuffIconPanel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = stuff != null ? stuff.Logo : EmptyIcon;

        _newStuff = stuff;
    }

    public void EndStuffChange()
    {
        StuffReplaceSelectPanel.SetActive(false);
        
        InChoice = false;
    }

    public void ReturnChange()
    {
        StuffReplaceSelectPanel.SetActive(false);
        StuffCharaSelectPanel.SetActive(true);
    }
}