using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class BattleStateMachine : MonoBehaviour
{
    public static BattleStateMachine instance = null;

    public enum PerformAction
    {
        WAIT,
        TAKEINPUT,
        PERFORMACTION, // idle state
        CHECKALIVE,
        WIN,
        LOSE
    }
    public PerformAction battleStates;

    public List<HandleTurn> PerformList = new List<HandleTurn>();

    public List<GameObject> HerosInBattle = new List<GameObject>();
    public List<GameObject> EnemysInBattle = new List<GameObject>();

    public List<GameObject> HerosToManage = new List<GameObject>();

    public bool isAuto = true;

    //public HeroInput heroInput;

    // win & lose panel
    public GameObject winPanel;
    public GameObject losePanel;

    // turn
    public int heroCnt = 0;
    public int enemyCnt = 0;

    public bool isTurnEnd;
    public enum WhosTurn
    {
        HERO,
        ENEMY
    }
    public WhosTurn turn;

    private void Awake()
    {
        // instance가 시스템 상에 존재하지 않는경우
        if (instance == null)
        {
            instance = this; // 인스턴스 생성
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 해당 객체가 아니면
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        battleStates = BattleStateMachine.PerformAction.WAIT;
    }

    void Start()
    {
        EnemysInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
    }

    void Update()
    {
        switch (battleStates)
        {
            // idle state
            case PerformAction.WAIT:
                break;

            case PerformAction.TAKEINPUT:

                battleStates = PerformAction.WAIT;
                // hero turn
                if (turn == WhosTurn.HERO)
                {
                    // 관리할 hero가 있는 경우
                    if (BattleStateMachine.instance.HerosToManage.Count > 0)
                    {
                        // input activate
                        HerosToManage[0].GetComponent<HeroInput>().input = HeroInput.HeroGUI.ACTIVATE;
                    }
                    else
                        Debug.Log("HeroToManage null");
                }
                // enemy turn
                else
                {
                    if (PerformList.Count > 0)
                    {
                        PerformList[0].AttacksGameObject.transform.Find("Selector").gameObject.SetActive(true);
                    }
                    
                }

                break;


            case PerformAction.PERFORMACTION:
                battleStates = BattleStateMachine.PerformAction.WAIT;

                if (turn == WhosTurn.HERO)
                {
                    HeroStateMachine HSM = PerformList[0].AttacksGameObject.GetComponent<HeroStateMachine>();
                    HSM.currentState = HeroStateMachine.TurnState.ACTION;
                }
                else
                {
                    EnemyStateMachine ESM = PerformList[0].AttacksGameObject.GetComponent<EnemyStateMachine>();
                    ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                }
                break;

            case PerformAction.CHECKALIVE:
                if (HerosInBattle.Count < 1)
                {
                    battleStates = PerformAction.LOSE;
                    // lose game
                }
                else if (EnemysInBattle.Count < 1)
                {
                    battleStates = PerformAction.WIN;
                    // win game
                }
                else
                {
                    //heroInput.actionUI.clearAttackPanel();
                    
                    //heroInput.input = HeroInput.HeroGUI.WAITING;
                    // call function
                }
                break;

            case PerformAction.LOSE:
                winPanel.SetActive(false);
                losePanel.SetActive(true);
                break;

            case PerformAction.WIN:
                winPanel.SetActive(true);
                losePanel.SetActive(false);
                for(int i = 0; i < HerosInBattle.Count; i++)
                {
                    HerosInBattle[i].GetComponent<HeroStateMachine>().currentState = HeroStateMachine.TurnState.WAITING;
                }
                break;

        }
    }
}
