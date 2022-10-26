using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyStateMachine : MonoBehaviour
{
    private BattleStateMachine BSM;
    public BaseEnemy enemy;

    //public GameObject turnCircleBar;
    public enum TurnState
    {
        PROCESSING, // bar is going to fill
        CHECKTURN,
        CHOOSEACTION,
        WAITING, // idle state
        ACTION,
        DONE,
        DEAD
    }

    public TurnState currentState;

    // for the ProgressBar
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    public Slider MPBar;
    public Slider HPBar;

    // this gameObject
    private Vector3 startPosition;

    // timeforaction
    private bool actionStarted = false;
    public GameObject HeroToAttack;

    // dead
    private bool alive = true;

    // Animation
    public Animator anim;
    private float animSpeed = 5f;

    public GameObject Selector;

    private void Start()
    {
        currentState = TurnState.PROCESSING;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        Selector.SetActive(false);
        startPosition = transform.position;
        anim = GetComponent<Animator>();

        HPBar.maxValue = enemy.baseHP;
        HPBar.value = enemy.curHP;
    }

    private void Update()
    {
        HPBar.value = enemy.curHP;
        switch (currentState)
        {
            case TurnState.PROCESSING:
                UpgradeProgressBar();
                break;

            case TurnState.CHECKTURN:
                if (BSM.turn == BattleStateMachine.WhosTurn.ENEMY)
                {
                    currentState = TurnState.CHOOSEACTION;
                }
                break;

            case TurnState.CHOOSEACTION:
                ChooseAction();
                currentState = TurnState.WAITING;
                break;

            case TurnState.WAITING: // idle state
                break;

            case TurnState.ACTION:
                Selector.SetActive(true);
                StartCoroutine(TimeForAction());
                
                break;
                
            case TurnState.DONE:
                break;

            case TurnState.DEAD:
                if (!alive)
                    return;
                else
                {
                    // change tag from Hero to DeadHero
                    this.gameObject.tag = "DeadEnemy";

                    // not attackable by hero
                    BSM.EnemysInBattle.Remove(this.gameObject);

                    // deactive the selector
                    Selector.SetActive(false);

                    if (BSM.EnemysInBattle.Count > 0)
                    {
                        // remove item from performlist
                        for (int i = 0; i < BSM.PerformList.Count; i++)
                        {
                            if (BSM.PerformList[i].AttacksGameObject == this.gameObject)
                            {
                                BSM.PerformList.Remove(BSM.PerformList[i]);
                            }
                            if (BSM.PerformList[i].AttackersTarget == this.gameObject)
                            {
                                BSM.PerformList[i].AttackersTarget = BSM.EnemysInBattle[Random.Range(0, BSM.EnemysInBattle.Count)];
                            }
                        }
                    }

                    // change color or play dead animation
                    anim.SetBool("isAlive", false);

                    alive = false;
                }
                StartCoroutine(Dead());
                break;

        }
    }

    void UpgradeProgressBar()
    {
        cur_cooldown += Time.deltaTime; // increase by running time
        MPBar.value = cur_cooldown / max_cooldown;
        
        if (cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.CHECKTURN;
        }
    }

    void UpdateEnemyBar()
    {
        HPBar.value = enemy.curHP;
    }

    void ChooseAction()
    {
        Debug.Log("chooseAction" + gameObject.name);
        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = this.gameObject.name; // this enemy name
        myAttack.Type = "Enemy";
        myAttack.AttacksGameObject = this.gameObject;

        // choose random target
        myAttack.AttackersTarget = BattleStateMachine.instance.HerosInBattle[Random.Range(0, BattleStateMachine.instance.HerosInBattle.Count)];
        HeroToAttack = myAttack.AttackersTarget;

        // choose random skill
        int num = Random.Range(0, enemy.basicSkill.Count); 
        myAttack.choosenAttack = enemy.basicSkill[num];

        BattleStateMachine.instance.PerformList.Add(myAttack);

        BattleStateMachine.instance.battleStates = BattleStateMachine.PerformAction.PERFORMACTION;
    }

    private IEnumerator TimeForAction()
    {
        // already started
        if (actionStarted)
        {
            yield break; // out of the Enuerator
        }

        actionStarted = true; // after doing, set actionStarted to be true

        // animate the enemy near the hero to attack
        Vector3 heroPosition = 
            new Vector3(
                HeroToAttack.transform.position.x+1f
                , HeroToAttack.transform.position.y
                , HeroToAttack.transform.position.z-2f
                );

        while (MoveTowardsHero(heroPosition)) { yield return null; }
        anim.SetBool("isAttack", true);
        anim.SetInteger("AttackType", Random.Range(1, 3));

        // do damage
        DoDamage();

        // wait abit
        yield return new WaitForSeconds(1f);
        anim.SetBool("isAttack", false);
        anim.SetInteger("AttackType", 0);
        

        // animate back to startposotion
        Vector3 firstPosition = startPosition;
        
        while (MoveTowardsStart(firstPosition)) { yield return null; }


        // remove this perfomer from the list in BSM
        BSM.PerformList.RemoveAt(0);

        currentState = TurnState.DONE;

        if (BSM.enemyCnt >= BSM.EnemysInBattle.Count-1)
        {
            BSM.turn = BattleStateMachine.WhosTurn.HERO;
            BSM.battleStates = BattleStateMachine.PerformAction.TAKEINPUT;
            BSM.enemyCnt = 0;
        }
        else
        {
            BSM.enemyCnt++;
            if (BattleStateMachine.instance.PerformList.Count > 0)
            {
                BattleStateMachine.instance.battleStates = BattleStateMachine.PerformAction.PERFORMACTION;
            }
        }

        // reset BSM -> Wait
        //BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
        
        // end coroutine
        actionStarted = false;

        // reset this enemy state
        cur_cooldown = 0f;
        currentState = TurnState.PROCESSING;
        Selector.SetActive(false);
        
    }

    private bool MoveTowardsHero(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    void DoDamage()
    {
        float calc_damage = enemy.curATK + BSM.PerformList[0].choosenAttack.skillDamage;
        HeroToAttack.GetComponent<HeroStateMachine>().TakeDamage(calc_damage);
        HeroToAttack.GetComponent<HeroStateMachine>().PlayGetHitAni();
    }

    public void TakeDamage(float getDamageAmount)
    {
        enemy.curHP -= getDamageAmount;
        if (enemy.curHP <= 0)
        {
            enemy.curHP = 0;
            currentState = TurnState.DEAD;
        }
        anim.SetBool("GetHit", true);
        
    }

    public void PlayGetHitAni()
    {
        anim.SetBool("GetHit", true);
        StartCoroutine(StopGetHitAni());
    }

    IEnumerator StopGetHitAni()
    {
        yield return new WaitForSeconds(2f);
        anim.SetBool("GetHit", false);
        UpdateEnemyBar();
    }

    IEnumerator Dead()
    {
        while (true)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Die") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                break;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        HPBar.gameObject.SetActive(false);
        MPBar.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}

