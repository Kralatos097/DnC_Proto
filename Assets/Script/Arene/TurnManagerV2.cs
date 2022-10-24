using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TurnManagerV2 : MonoBehaviour
{
    [SerializeField] private Transform CombatEndCanvas;
    private static Transform _combatEndCanvas;
    
    private static Queue<TacticsMovement> turnOrder = new Queue<TacticsMovement>();
    private static List<TacticsMovement> unitsList = new List<TacticsMovement>();

    [HideInInspector] public bool startCombat = false;
    public bool bossFight = false;
    private static bool _combatEnded = false;
    private static bool _isDefeat = false;

    private void Start()
    {
        Invoke(nameof(LateStart), 1);
        _combatEnded = false;
        _combatEndCanvas = CombatEndCanvas;
    }
    
    private void LateStart()
    {
        StartCombat();
    }

    void StartCombat()
    {
        ListToQueue();
        StartTurn();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && _combatEnded)
        {
            
            if(!bossFight)
            {
                if(_isDefeat)
                {
                    //todo: loose screen
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("ProtoDJ"));
                    SceneManager.LoadScene("DefeatScene");
                }
                else
                {
                    SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                    DonjonManager._gameContainer.SetActive(true);
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName("ProtoDJ"));
                }
                
            }
            else
            {
                if(_isDefeat)
                {
                    //todo: loose screen
                    SceneManager.LoadScene("DefeatScene");
                }
                else
                {
                    //todo: Session end
                    SceneManager.LoadScene("VictoryScene");
                }
            }
        }
    }

    private static void EndCombat(bool pStatue)
    {
        _combatEnded = true;
        //Victoire player
        if(pStatue)
        {
            //todo
            Debug.Log("Victiore");
            _combatEndCanvas.GetChild(0).gameObject.SetActive(true);
            //Todo: changement de scene apres un clic
        }
        //Défaite player
        else
        {
            //todo
            Debug.Log("Defeat");
            _isDefeat = true;
            _combatEndCanvas.GetChild(1).gameObject.SetActive(true);
            //Todo: changement de scene apres un clic
        }
    }
    
    static void StartTurn()
    {
        if (ArePlayersAlive() && AreEnnemisAlive())
        {
            while(!turnOrder.Peek().GetComponent<CombatStat>().isAlive)
            {
                TacticsMovement DeadUnit = turnOrder.Dequeue();
                Debug.Log("Dead");
                Destroy(DeadUnit.gameObject);
            }
            Debug.Log("Turn of : " + turnOrder.Peek().name);
            
            turnOrder.Peek().BeginTurn();
        }
        else
        {
            EndCombat(ArePlayersAlive());
        }
    }

    public static void EndTurn()
    {
        TacticsMovement unit = turnOrder.Dequeue();
        unit.EndTurn();
        unit.EquipCDMinus(1);
        turnOrder.Enqueue(unit);
        TacticsMovement.PlayersTurn = false;
        StartTurn();
    }

    private static void ListToQueue()
    {
        while (unitsList.Count > 0)
        {
            int temp = -100;

            TacticsMovement unitRet = null;
            foreach (TacticsMovement unit in unitsList)
            {
                int init = unit.gameObject.GetComponent<CombatStat>().currInit;

                //si l'init est surerieur échange
                if (temp < init)
                {
                    temp = init;
                    unitRet = unit;
                }

                //si similaire on départage
                if (temp == init)
                {
                    int newBaseInit = unit.gameObject.GetComponent<CombatStat>().initiative;
                    int retBaseInit = unitRet.gameObject.GetComponent<CombatStat>().initiative;

                    //si l'init est surerieur échange
                    if (retBaseInit < newBaseInit)
                    {
                        temp = init;
                        unitRet = unit;
                    }
                    //si égale on départage
                    else if (retBaseInit == newBaseInit)
                    {
                        //on prioritise le player
                        if ((unit.gameObject.CompareTag("Player") && !unitRet.gameObject.CompareTag("Player"))
                            || (!unit.gameObject.CompareTag("Player") && unitRet.gameObject.CompareTag("Player")))
                        {
                            temp = init;
                            unitRet = unit;
                        }
                        //Si les 2 parties sont dans la meme équipe on choisie au hasard
                        else if (unit.gameObject.CompareTag("Player") == unitRet.gameObject.CompareTag("Player"))
                        {
                            int t = Random.Range(0, 2);
                            if (t == 0)
                            {
                                temp = init;
                                unitRet = unit;
                            }
                        }
                    }
                }
            }

            unitsList.Remove(unitRet);
            turnOrder.Enqueue(unitRet);
        }
    }
    
    public static void AddUnit(TacticsMovement unit)
    {
         unitsList.Add(unit);
    }
    
    //todo: remove of the list

    private static bool ArePlayersAlive()
    {
        foreach (TacticsMovement unit in turnOrder)
        {
            /*if (unit.gameObject.CompareTag("Player") && !unit.gameObject.GetComponent<CombatStat>().isAlive)
            {
                turnOrder.Dequeue();
            }*/
            if(unit.gameObject.CompareTag("Player") && unit.gameObject.GetComponent<CombatStat>().isAlive)
            {
                return true;
            }
        }

        return false;
    }
    
    private static bool AreEnnemisAlive()
    {
        foreach (TacticsMovement unit in turnOrder)
        {
            /*if (unit.gameObject.CompareTag("Ennemi") && !unit.gameObject.GetComponent<CombatStat>().isAlive)
            {
                turnOrder.Dequeue();
            }*/
            if (unit.gameObject.CompareTag("Ennemi") && unit.gameObject.GetComponent<CombatStat>().isAlive)
            {
                return true;
            }
        }
        return false;
    }
}
