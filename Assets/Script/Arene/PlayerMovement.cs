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

    public Perso charaClass;
    
    [SerializeField] protected PlayerBaseInfo UnitInfo;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        Init();
        TurnManagerV2.AddPlayerToList(this);
        _lastPos = transform.position;
        _lastRot = transform.rotation;
    }

    protected override void GetUnitInfo()
    {
        CombatStat _combatStat = gameObject.GetComponent<CombatStat>();
        switch(charaClass)
        {
            case Perso.Warrior:
                _combatStat.MaxHp = WarriorInfo.MaxHp;
                _combatStat.currHp = WarriorInfo.MaxHp;
                _combatStat.initiative = WarriorInfo.Init;

                move = WarriorInfo.Movement;
                equipmentOne = WarriorInfo.EquipmentOne;
                equipmentTwo = WarriorInfo.EquipmentTwo;
                passif = WarriorInfo.Passif;
                consummable = WarriorInfo.Consumable;
                break;
            case Perso.Thief:
                _combatStat.MaxHp = ThiefInfo.MaxHp;
                _combatStat.currHp = ThiefInfo.MaxHp;
                _combatStat.initiative = ThiefInfo.Init;

                move = ThiefInfo.Movement;
                equipmentOne = ThiefInfo.EquipmentOne;
                equipmentTwo = ThiefInfo.EquipmentTwo;
                passif = ThiefInfo.Passif;
                consummable = ThiefInfo.Consumable;
                break;
            case Perso.Cleric:
                _combatStat.MaxHp = ClericInfo.MaxHp;
                _combatStat.currHp = ClericInfo.MaxHp;
                _combatStat.initiative = ClericInfo.Init;

                move = ClericInfo.Movement;
                equipmentOne = ClericInfo.EquipmentOne;
                equipmentTwo = ClericInfo.EquipmentTwo;
                passif = ClericInfo.Passif;
                consummable = ClericInfo.Consumable;
                break;
            case Perso.Wizard:
                _combatStat.MaxHp = WizardInfo.MaxHp;
                _combatStat.currHp = WizardInfo.MaxHp;
                _combatStat.initiative = WizardInfo.Init;

                move = WizardInfo.Movement;
                equipmentOne = WizardInfo.EquipmentOne;
                equipmentTwo = WizardInfo.EquipmentTwo;
                passif = WizardInfo.Passif;
                consummable = WizardInfo.Consumable;
                break;
            case Perso.Default:
                _combatStat.MaxHp = UnitInfo.MaxHp;
                _combatStat.currHp = UnitInfo.MaxHp;
                _combatStat.initiative = UnitInfo.Initiative;

                move = UnitInfo.Movement;
                equipmentOne = UnitInfo.equipmentOne;
                equipmentTwo = UnitInfo.equipmentTwo;
                passif = UnitInfo.passif;
                consummable = UnitInfo.consumable;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetUnitInfo()
    {
        Debug.Log(charaClass);
        
        switch(charaClass)
        {
            case Perso.Warrior:
                WarriorInfo.MaxHp = _combatStat.MaxHp;
                WarriorInfo.CurrentHp = _combatStat.currHp;
                WarriorInfo.Init = _combatStat.initiative;
                
                WarriorInfo.Consumable = consummable;
                break;
            case Perso.Thief:
                ThiefInfo.MaxHp = _combatStat.MaxHp;
                ThiefInfo.CurrentHp = _combatStat.currHp;
                ThiefInfo.Init = _combatStat.initiative;
                
                ThiefInfo.Consumable = consummable;
                break;
            case Perso.Cleric:
                ClericInfo.MaxHp = _combatStat.MaxHp;
                ClericInfo.CurrentHp = _combatStat.currHp;
                ClericInfo.Init = _combatStat.initiative;
                
                ClericInfo.Consumable = consummable;
                break;
            case Perso.Wizard:
                WizardInfo.MaxHp = _combatStat.MaxHp;
                WizardInfo.CurrentHp = _combatStat.currHp;
                WizardInfo.Init = _combatStat.initiative;
                
                WizardInfo.Consumable = consummable;
                break;
            case Perso.Default:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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
                _lastPos = transform.position;
                _lastRot = transform.rotation;
                TurnManagerV2.EndTurn();
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
        base.EndOfAttack();
        _lastPos = transform.position;
        _lastRot = transform.rotation;
        _uiManager.Reset();
        TurnManagerV2.EndTurn();
    }
}
