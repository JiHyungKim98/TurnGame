
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour
{
    private BattleStateMachine BSM;
    public BaseHero hero; // inspector to enter info

    public enum TurnState
    {
        PROCESSING, // bar is going to fill
        ADDTOLIST,
        CHECKTURN,
        WAITING, // idle state
        ACTION,
        DONE, // action end
        DEAD
    }

    public TurnState currentState;

    // ProgressBar
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    public Slider MPBar;
    public Slider HPBar;

    public GameObject Selector;

    // IEnumerator
    private bool actionStarted = false;
    public GameObject EnemyToAttack;
    private Vector3 startPosition;

    // Animation
    public Animator anim;
    private float animSpeed = 10f;
    private int animNum = 0;

    // dead
    private bool alive = true;

    // hero panel
    private AttackPanelStats stats;
    public GameObject HeroPanel;
    private Transform HeroPanelSpacer;

    // attack
    public int attackStrength = 1;

    // auto mode
    //public bool isAutoMode = false;

    // skill count
    lateSkillCount skillcnt = new lateSkillCount();

    HeroInput heroInput;

    
    private void Start()
    {
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();

        startPosition = transform.position;

        cur_cooldown = Random.Range(0, 2.5f);

        Selector.SetActive(false);
        
        currentState = TurnState.PROCESSING;

        anim = GetComponent<Animator>();
        anim.SetBool("isAlive", true);

        HPBar.maxValue = hero.baseHP;
        HPBar.value = hero.curHP;

        heroInput= GameObject.Find("BattleManager").GetComponent<HeroInput>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case TurnState.PROCESSING:
                UpgradeProgressBar();
                break;

            case TurnState.ADDTOLIST:
                BSM.HerosToManage.Add(this.gameObject);
                if (BattleStateMachine.instance.HerosToManage[0] == this.gameObject)
                {
                    //Selector.SetActive(true);
                    BattleStateMachine.instance.battleStates = BattleStateMachine.PerformAction.TAKEINPUT;
                }
                currentState = TurnState.CHECKTURN;
                break;

            case TurnState.CHECKTURN:
                if (BSM.turn == BattleStateMachine.WhosTurn.HERO)
                {
                    //Selector.SetActive(true);
                    currentState = TurnState.WAITING;
                }
                    
                break;

            case TurnState.WAITING: // idle state
                break;

            case TurnState.ACTION:
                StartCoroutine(TimeForAction());
                break;

            case TurnState.DONE: // action end
                heroActionDone();
                break;

            case TurnState.DEAD:
                if (!alive)
                    return;
                else
                {
                    this.gameObject.tag = "DeadHero";

                    BSM.HerosInBattle.Remove(this.gameObject);

                    BSM.HerosToManage.Remove(this.gameObject);

                    Selector.SetActive(false);

                    if (BSM.HerosInBattle.Count > 0)
                    {
                        for (int i = 0; i < BSM.PerformList.Count; i++)
                        {
                            if (BSM.PerformList[i].AttacksGameObject == this.gameObject)
                            {
                                BSM.PerformList.Remove(BSM.PerformList[i]);
                            }
                            if (BSM.PerformList[i].AttackersTarget == this.gameObject)
                            {
                                BSM.PerformList[i].AttackersTarget = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];
                            }
                        }
                    }

                    anim.SetBool("isAlive", false);

                    BSM.battleStates = BattleStateMachine.PerformAction.CHECKALIVE;
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
            currentState = TurnState.ADDTOLIST;
        }
    }


    private IEnumerator TimeForAction()
    {
        // already started
        if (actionStarted)
        {
            yield break; // out of the Enuerator
        }

        actionStarted = true; // after doing, set actionStarted to be true

        if (BSM.PerformList[0].AttackersTarget != null)
        {
            // animate the enemy near the hero to attack
            Vector3 enemyPosition =
                new Vector3(
                    EnemyToAttack.transform.position.x - 1f
                    , EnemyToAttack.transform.position.y
                    , EnemyToAttack.transform.position.z + 2f
                    );

            while (MoveTowardsEnemy(enemyPosition)) { yield return null; }

            anim.SetBool("isAttack", true);
            anim.SetInteger("AttackType", animNum);

            // do damage
            if (BSM.PerformList[0].choosenAttack == hero.SpecialSkill[0])
                DoDamage(true);

            else
                DoDamage(false);

            // wait abit
            yield return new WaitForSeconds(1f);
            anim.SetBool("isAttack", false);
            anim.SetInteger("AttackType", 0);



            // animate back to startposotion
            Vector3 firstPosition = startPosition;

            while (MoveTowardsStart(firstPosition)) { yield return null; }
        }
        
        currentState = TurnState.DONE;

        // end coroutine
        actionStarted = false;
    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    public void DoAttack(SkillInfo choosenAttack)
    {
        if (choosenAttack.GetType() == typeof(Spin))
        {
            if (hero.basicSkill.Find(x=>x.GetType()==typeof(Spin)).GetComponent<Spin>().isSuccess())
            {
                // mp full
                MPBar.value = hero.baseMP;
                BSM.heroCnt--;
                BSM.HerosToManage.Insert(0,this.gameObject);
            }
            else
                Debug.Log("spin fail");
        }
        else if(choosenAttack.GetType() == typeof(Sting))
        {
            if (hero.basicSkill.Find(x => x.GetType() == typeof(Sting)).GetComponent<Sting>().isSuccess())
                attackStrength = 2;
            else
                Debug.Log("sting fail");
        }
        else
        {
            attackStrength = 1;
        }
    }

    // do damage
    void DoDamage(bool isSpecialSkill)
    {
        float calc_damage = 0f;
        if (!isSpecialSkill)
            calc_damage = hero.curATK + BSM.PerformList[0].choosenAttack.skillDamage* attackStrength;

        EnemyToAttack.GetComponent<EnemyStateMachine>().TakeDamage(calc_damage);
        EnemyToAttack.GetComponent<EnemyStateMachine>().PlayGetHitAni();
    }
    public void TakeDamage(float getDamageAmount)
    {
        hero.curHP -= getDamageAmount;
        if (hero.curHP <= 0)
        {
            hero.curHP = 0;
            currentState = TurnState.DEAD;
        }
    }

    public void PlayGetHitAni()
    {
        anim.SetBool("GetHit", true);
        StartCoroutine(StopGetHitAni());
    }
    IEnumerator StopGetHitAni()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("GetHit", false);
        UpdateHeroBar();
    }

    void CreateHeroPanel()
    {
        HeroPanel = Instantiate(HeroPanel) as GameObject;
        stats = HeroPanel.GetComponent<AttackPanelStats>();
        
    }

    public void UpdateHeroBar()
    {
        HPBar.value = hero.curHP;
    }

    public void UpdateAnim(string skillName)
    {
        switch (skillName)
        {
            case "ManSlash":
                animNum = 1;
                break;

            case "WomanSlash":
                animNum = 2;
                break;

            case "Sting":
                animNum = 3;
                break;

            case "Spin":
                animNum = 4;
                break;
        }
    }

    public void ShowSpecialSkillParticle()
    {
        this.gameObject.transform.Find("SpecialSkillParticle").gameObject.SetActive(true);
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        this.gameObject.transform.Find("SpecialSkillParticle").gameObject.SetActive(false);
    }
    void heroActionDone()
    {
        BSM.PerformList.RemoveAt(0);
        
        // reset BSM -> Wait
        if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
        {
            BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
            cur_cooldown = 0f;
            currentState = TurnState.PROCESSING;
        }
        else
        {
            currentState = TurnState.WAITING;
        }

        if (BSM.heroCnt >= BSM.HerosInBattle.Count-1)
        {
            BSM.turn = BattleStateMachine.WhosTurn.ENEMY;
            BattleStateMachine.instance.battleStates = BattleStateMachine.PerformAction.TAKEINPUT;
            BSM.heroCnt = 0;
            skillcnt.skillLimitDesc();
        }
        else
        {
            BSM.heroCnt++;
            if (BattleStateMachine.instance.HerosToManage.Count > 0)
            {
                BattleStateMachine.instance.battleStates = BattleStateMachine.PerformAction.TAKEINPUT;
            }
            //BSM.HeroInput = HeroInput.HeroGUI.ACTIVATE;
        }
        
        currentState = TurnState.PROCESSING;
        
    }
    IEnumerator Dead()
    {
        while (true)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Die01_SwordAndShield") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                break;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        HPBar.gameObject.SetActive(false);
        MPBar.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
    
    // return this gameObject
    public GameObject thisHero()
    {
        return this.gameObject;
    }
}
