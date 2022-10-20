using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : TacticsMovement
{
    private UIManager _uiManager;
    private bool pass = false;
    private Vector3 _lastPos;
    private Quaternion _lastRot;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        Init();
        _lastPos = transform.position;
        _lastRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(!turn) return;
        PlayersTurn = true;

        //display action Selector on turn start
        if (!pass)
        {
            _uiManager.ShowActionSelector();
            pass = true;
        }

        switch(_uiManager.actionSelected)
        {
            case Action.Attack :
                _uiManager.HideActionSelector();
                _uiManager.SetStuff(equipmentOne,equipmentTwo,consummable);
                _uiManager.SetCd(EquiOneCD, EquiTwoCD);
                _uiManager.ShowEquipSelector();
                _uiManager.actionSelected = Action.Equip;
                /*AffAttackRange();
                CheckAttack();*/
                break;
            case Action.Move:
                _uiManager.HideActionSelector();
                
                if(!moving)
                {
                    //Début du Soulevement du pion lors du mouvement
                    if(!passM)
                    {
                        transform.GetChild(0).Translate(0, MoveY, 0);
                        passM = true;
                    }
                    
                    FindSelectableTile();
                    CheckMove();
                }
                else
                {
                    Move();
                    _uiManager.alreadyMoved = true;
                }
                break;
            case Action.Stay:
                TurnManagerV2.EndTurn();
                _lastPos = transform.position;
                _lastRot = transform.rotation;
                pass = false;
                _uiManager.Reset();
                break;
            case Action.Default:
                RemoveSelectableTile();
                if(passM)
                {
                    //Fin du Soulevement du pion lors du mouvement
                    transform.GetChild(0).Translate(0,-MoveY,0);
                    passM = false;
                }
                break;
            case Action.CancelMove:
                transform.position = _lastPos;
                transform.rotation = _lastRot;
                
                _uiManager.alreadyMoved = false;
                _uiManager.actionSelected = Action.Default;
                _uiManager.ShowActionSelector();
                break;
            case Action.Equip:
                _uiManager.HideActionSelector();
                switch(_uiManager.stuffSelected)
                {
                    case StuffSelected.EquipOne:
                        atkRange = equipmentOne.Range;
                        break;
                    case StuffSelected.EquipTwo:
                        atkRange = equipmentTwo.Range;
                        break;
                    case StuffSelected.Consum:
                        atkRange = consummable.Range;
                        break;
                    case StuffSelected.Default:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if(_uiManager.stuffSelected != StuffSelected.Default)
                {
                    _uiManager.HideEquipSelector();
                    AffAttackRange();
                    CheckAttack();
                }
                break;
            default:
                break;
        }
    }

    private void CheckMove()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay((Input.mousePosition));

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if (t.selectable)
                    {
                        MoveToTile(t);
                    }
                }
            }
        }
    }
    
    private void CheckAttack()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay((Input.mousePosition));

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    Debug.Log(t.GetGameObjectOnTop());

                    bool passAtk = false;
                    GameObject TargetGO = t.GetGameObjectOnTop();
                    if(TargetGO != null) passAtk = (TargetGO.CompareTag("Ennemi")||TargetGO.CompareTag("Player"));

                    if (t.selectable && passAtk)
                    {
                        int equip = 0;
                        switch(_uiManager.stuffSelected)
                        {
                            
                            case StuffSelected.EquipOne:
                                equip = 1;
                                break;
                            case StuffSelected.EquipTwo:
                                equip = 2;
                                break;
                            case StuffSelected.Consum:
                                equip = 3;
                                break;
                            case StuffSelected.Default:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        
                        Attack(TargetGO.GetComponent<CombatStat>(), equip);
                    }
                }
                //todo: else pour si on click sur la figurine
            }
        }
    }
    
    protected override void EndOfMovement()
    {
        base.EndOfMovement();
        _uiManager.actionSelected = Action.Default;
        _uiManager.ShowActionSelector();
    }
    
    protected override void EndOfAttack()
    {
        _uiManager.Reset();
        TurnManagerV2.EndTurn();
    }
}
