using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : TacticsMovement
{
    private UIManager _uiManager;
    private bool pass = false;
    private Vector3 _lastPos;
    
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        Init();
        _lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!turn) return;

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
                AttackRange();
                CheckAttack();
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
                _uiManager.alreadyMoved = false;
                _uiManager.actionSelected = Action.Default;
                _uiManager.ShowActionSelector();
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

                    bool passAtk = t.GetGameObjectOnTop() != null;

                    if (t.selectable && passAtk)
                    {
                        Attack();
                    }
                }
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
