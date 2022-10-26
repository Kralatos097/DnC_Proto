using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [HideInInspector] public Action actionSelected = Action.Default;
    private bool _actionSelectorShown = false;
    [HideInInspector] public bool alreadyMoved = false;

    [Header("Drag'n Drop")]
    [SerializeField] private GameObject ActionSelectorPanel;
    [SerializeField] private Button MoveButton;
    [SerializeField] private GameObject EquipSelectorPanel;
    
    [HideInInspector] public StuffSelected stuffSelected = StuffSelected.Default;
    /*private bool _equipSelectorShown = false;*/

    private Equipment equipmentOne = null;
    private int equipOneCd = 0;
    
    private Equipment equipmentTwo = null;
    private int equipTwoCd = 0;

    private Consummable consummable = null;

    [Header("Init Aff")]
    public Transform InitPanel;
    public GameObject PlayerInitPanel;
    public GameObject WarriorInitPanel;
    public GameObject ThiefInitPanel;
    public GameObject ClericInitPanel;
    public GameObject WizardInitPanel;
    public GameObject EnemyInitPanel;

    public static Action<GameObject> setInitAction;

    private void Awake()
    {
        setInitAction = AddUnitInitUi;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !_actionSelectorShown && !EventSystem.current.IsPointerOverGameObject() && TacticsMovement.PlayersTurn)
        {
            //todo mieux gerer quand on clique en dehors des ranges
            ShowActionSelector();
        }
            
        if (Input.GetMouseButtonUp(1))//On release right click
        {
            switch (_actionSelectorShown)
            {
                case true:
                    HideActionSelector();
                    break;
                case false:
                    switch (actionSelected)
                    {
                        case Action.Default:
                            if (alreadyMoved)
                            {
                                actionSelected = Action.CancelMove;
                            }
                            return;
                        case Action.Move:
                            actionSelected = Action.Default;
                            ShowActionSelector();
                            break;
                        case Action.Attack:
                            actionSelected = Action.Default;
                            ShowActionSelector();
                            break;
                        case Action.Stay:
                            actionSelected = Action.Default;
                            ShowActionSelector();
                            break;
                        case Action.Equip:
                            actionSelected = Action.Default;
                            HideEquipSelector();
                            stuffSelected = StuffSelected.Default;
                            ShowActionSelector();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
            }
        }
    }
    
    public void ShowActionSelector()
    {
        _actionSelectorShown = true;
        ActionSelectorPanel.SetActive(true);
        
        /*MoveButton.interactable = !alreadyMoved;*/
        MoveButton.GetComponentInChildren<TextMeshProUGUI>().text = alreadyMoved ? "Return" : "Move";
    }

    public void HideActionSelector()
    {
        _actionSelectorShown = false;
        ActionSelectorPanel.SetActive(false);
    }
    
    public void ShowEquipSelector()
    {
        /*_equipSelectorShown = true;*/
        EquipSelectorPanel.SetActive(true);

        if (equipmentOne == null)
        {
            EquipSelectorPanel.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
            EquipSelectorPanel.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            EquipSelectorPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
        else if(equipOneCd > 0)
        {
            EquipSelectorPanel.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = false;
            EquipSelectorPanel.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = ""+equipOneCd;
            EquipSelectorPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
            EquipSelectorPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = equipmentOne.Logo;
        }
        else
        {
            EquipSelectorPanel.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = true;
            EquipSelectorPanel.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            EquipSelectorPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
            EquipSelectorPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = equipmentOne.Logo;
        }
        if (equipmentTwo == null)
        {
            EquipSelectorPanel.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            EquipSelectorPanel.transform.GetChild(1).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            EquipSelectorPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
        else if (equipTwoCd > 0)
        {
            EquipSelectorPanel.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = false;
            EquipSelectorPanel.transform.GetChild(1).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = ""+equipTwoCd;
            EquipSelectorPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
            EquipSelectorPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().sprite = equipmentTwo.Logo;
        }
        else
        {
            EquipSelectorPanel.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = true;
            EquipSelectorPanel.transform.GetChild(1).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";
            EquipSelectorPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
            EquipSelectorPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Image>().sprite = equipmentTwo.Logo;
        }
        if (consummable == null)
        {
            EquipSelectorPanel.transform.GetChild(2).gameObject.GetComponent<Button>().interactable = false;
            EquipSelectorPanel.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
        else
        {
            EquipSelectorPanel.transform.GetChild(2).gameObject.GetComponent<Button>().interactable = true;
            EquipSelectorPanel.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
            EquipSelectorPanel.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Image>().sprite = consummable.Logo;
        }
    }


    public void HideEquipSelector()
    {
        /*_equipSelectorShown = false;*/
        EquipSelectorPanel.SetActive(false);
    }

    public void MoveSelected()
    {
        if (alreadyMoved)
        {
            actionSelected = Action.CancelMove;
            HideActionSelector();
        }
        else
        {
            actionSelected = Action.Move;
            HideActionSelector();
        }
    }
    
    public void AttackSelected()
    {
        actionSelected = Action.Attack;
        HideActionSelector();
    }
    
    public void StaySelected()
    {
        actionSelected = Action.Stay;
        HideActionSelector();
    }

    public void Reset()
    {
        _actionSelectorShown = false;
        actionSelected = Action.Default;
        stuffSelected = StuffSelected.Default;
        alreadyMoved = false;
    }

    public void WeaponSelectionEquipOne()
    {
        stuffSelected = StuffSelected.EquipOne;
    }

    public void WeaponSelectionEquipTwo()
    {
        stuffSelected = StuffSelected.EquipTwo;
    }

    public void WeaponSelectionEquipConsum()
    {
        stuffSelected = StuffSelected.Consum;
    }

    public void SetStuff(Equipment equipOne, Equipment equipTwo, Consummable consum)
    {
        equipmentOne = equipOne;
        equipmentTwo = equipTwo;
        consummable = consum;
    }

    public void SetCd(int CdOne, int CdTwo)
    {
        equipOneCd = CdOne;
        equipTwoCd = CdTwo;
    }

    private void AddUnitInitUi(GameObject unit)
    {
        PlayerMovement playerMovement = unit.GetComponent<PlayerMovement>();
        CombatStat combatStat = unit.GetComponent<CombatStat>();
        GameObject t;
        if (playerMovement != null)
        {
            switch (playerMovement.charaClass)
            {
                case Perso.Default:
                    t = Instantiate(PlayerInitPanel, InitPanel);
                    break;
                case Perso.Warrior:
                    t = Instantiate(WarriorInitPanel, InitPanel);
                    break;
                case Perso.Thief:
                    t = Instantiate(ThiefInitPanel, InitPanel);
                    break;
                case Perso.Cleric:
                    t = Instantiate(ClericInitPanel, InitPanel);
                    break;
                case Perso.Wizard:
                    t = Instantiate(WizardInitPanel, InitPanel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            t.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + combatStat.currHp;
            t.transform.GetChild(1).Find("FillHpImg").GetComponent<Image>().fillAmount =
                (combatStat.currHp / (float)combatStat.MaxHp);
            if (playerMovement.GetPassif() == null)
            {
                t.transform.Find("PassifImg").gameObject.SetActive(false);
            }
            else
                t.transform.Find("PassifImg").GetComponent<Image>().sprite = playerMovement.GetPassif().Logo;
        }
        else
        {
            //NPCMove npcMove = unit.GetComponent<NPCMove>();
            t = Instantiate(EnemyInitPanel, InitPanel);
        }
        t.transform.Find("ArmorImg").gameObject.SetActive(false);
        t.transform.Find("StatusImg").gameObject.SetActive(false);
    }
}