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
    [SerializeField] private GameObject ActionSelectorCanvas;
    [SerializeField] private Button MoveButton;


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
        ActionSelectorCanvas.SetActive(true);
        
        /*MoveButton.interactable = !alreadyMoved;*/
        MoveButton.GetComponentInChildren<TextMeshProUGUI>().text = alreadyMoved ? "Return" : "Move";
        
    }

    public void HideActionSelector()
    {
        _actionSelectorShown = false;
        ActionSelectorCanvas.SetActive(false);
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
        alreadyMoved = false;
    }
}

public enum Action
{
    Default,
    Move,
    Attack,
    Stay,
    CancelMove,
}