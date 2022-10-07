using System;
using System.Collections;
using System.Collections.Generic;
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
    private bool _equipSelectorShown = false;

    private Equipment equipmentOne = null;
    private int equipOneCd = 0;
    
    private Equipment equipmentTwo = null;
    private int equipTwoCd = 0;

    private Consummable consummable = null;


    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !_actionSelectorShown && !EventSystem.current.IsPointerOverGameObject())
            ShowActionSelector();
            
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
        _equipSelectorShown = true;
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
            EquipSelectorPanel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = equipmentTwo.Logo;
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
        }else
        {
            EquipSelectorPanel.transform.GetChild(2).gameObject.GetComponent<Button>().interactable = true;
            EquipSelectorPanel.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
            EquipSelectorPanel.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Image>().sprite = consummable.Logo;
        }
    }


    public void HideEquipSelector()
    {
        _equipSelectorShown = false;
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
}