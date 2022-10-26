using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class HeroInput : MonoBehaviour
{
    public enum HeroGUI
    {
        ACTIVATE,
        WAITING,
        DONE
    }
    public HeroGUI input;

    private HandleTurn HeroChoice=new HandleTurn();
    public HeroActionUI actionUI;

    lateSkillCount skillCount;

    private void Awake()
    {
        input = HeroGUI.WAITING;
    }

    private void Start()
    {
        skillCount = new lateSkillCount();
        actionUI = GameObject.Find("ActionPanel").GetComponent<HeroActionUI>();
    }

    private void Update()
    {
        switch (input)
        {
            case HeroGUI.ACTIVATE:
                if (BattleStateMachine.instance.HerosToManage.Count > 0)
                {
                    input = HeroGUI.WAITING;
                    this.gameObject.transform.Find("Selector").gameObject.SetActive(true);
                    // auto input check
                    if (BattleStateMachine.instance.isAuto)
                    {
                        actionUI.SetUI(false);
                        AutoInput();
                    }
                    else
                    {
                        actionUI.SetUI(true);
                        actionUI.CreateAttackButtons();
                    }
                }
                
                break;

                // idle state
            case HeroGUI.WAITING:
                break;

            case HeroGUI.DONE:
                HeroInputDone();
                input = HeroGUI.WAITING;
                break;
        }
    }

    #region auto Input
    // (0) auto selection
    public void AutoInput()
    {
        HeroStateMachine HSM = BattleStateMachine.instance.HerosToManage[0].GetComponent<HeroStateMachine>();

        HeroChoice.Attacker = gameObject.name; // this enemy name
        HeroChoice.Type = "Hero";
        HeroChoice.AttacksGameObject = gameObject;
        HeroChoice.choosenAttack = chooseSkill(HSM);

        if(HeroChoice.choosenAttack.GetType()==typeof(Snow)|| HeroChoice.choosenAttack.GetType() == typeof(Heal))
        {
            HeroChoice.AttackersTarget = null;
            StartCoroutine(DoSpecialSkill()); 
        }
        else
        {
            HeroChoice.AttackersTarget = ChooseAttackTarget();
            StartCoroutine(DoBasicSkill(HSM));
        }
    }

    SkillInfo chooseSkill(HeroStateMachine HSM)
    {
        string skillname = "";
        if (HSM.hero.SpecialSkill.Find(x => x.GetType() == typeof(Snow)))
        {
            skillname = HSM.hero.SpecialSkill.Find(x => x.GetType() == typeof(Snow)).name;
            if (skillCount.CheckSkillLimit(skillname) == 0)
            {
                if(BattleStateMachine.instance.EnemysInBattle.Find(x => x.GetComponent<EnemyStateMachine>().enemy.curDEF != 0) != null)
                {
                    skillCount.setSkillLimit(skillname);
                    return HSM.hero.SpecialSkill.Find(x=>x.GetType()==typeof(Snow)); 
                }
            }

            skillname = HSM.hero.basicSkill.Find(x => x.GetType() == typeof(Spin)).name;
            if (skillCount.CheckSkillLimit(skillname) == 0)
            {
                skillCount.setSkillLimit(skillname);
                return HSM.hero.basicSkill.Find(x => x.GetType() == typeof(Spin));
            }
            else
                return HSM.hero.basicSkill.Find(x => x.GetType() == typeof(ManSlash));
        }

        // 2. hero woman case
        else if (HSM.hero.SpecialSkill.Find(x => x.GetType() == typeof(Heal)))
        {
            skillname = HSM.hero.SpecialSkill.Find(x => x.GetType() == typeof(Heal)).name;
            if (skillCount.CheckSkillLimit(skillname) == 0)
            {
                if (BattleStateMachine.instance.HerosInBattle.Find(x => x.GetComponent<HeroStateMachine>().hero.curHP <= x.GetComponent<HeroStateMachine>().hero.baseHP / 2) != null)
                {
                    skillCount.setSkillLimit(skillname);
                    return HSM.hero.SpecialSkill.Find(x => x.GetType() == typeof(Heal));
                }
            }

            skillname = HSM.hero.basicSkill.Find(x => x.GetType() == typeof(Sting)).name;
            if (skillCount.CheckSkillLimit(skillname) == 0)
            {
                skillCount.setSkillLimit(skillname);
                return HSM.hero.basicSkill.Find(x => x.GetType() == typeof(Sting));
            }
            else
                return HSM.hero.basicSkill.Find(x => x.GetType() == typeof(WomanSlash));
        }
        return null;
    }

    GameObject ChooseAttackTarget()
    {
        float minHP = float.MaxValue;
        GameObject target = null;

        foreach (var enemy in BattleStateMachine.instance.EnemysInBattle)
        {
            if (minHP > enemy.GetComponent<EnemyStateMachine>().enemy.curHP)
                target = enemy;
        }
        return target;
    }
    #endregion

    #region special skill input
    public void InputSpecialSkill(string skillName)
    {
        HeroStateMachine HSM = BattleStateMachine.instance.HerosToManage[0].GetComponent<HeroStateMachine>();
        int index = HSM.hero.SpecialSkill.FindIndex(x => x.name.Equals(skillName));

        HeroChoice.Attacker = BattleStateMachine.instance.HerosToManage[0].name;
        HeroChoice.AttacksGameObject = BattleStateMachine.instance.HerosToManage[0];
        HeroChoice.Type = "Hero";
        HeroChoice.choosenAttack = HSM.hero.SpecialSkill[index];

        StartCoroutine( DoSpecialSkill());

        actionUI.SetUI(false);
    }

    

    #endregion

    #region basic skill Input
    // (1) hero input - skill selection
    public void InputBasicSkill(string skillName)
    {
        HeroStateMachine HSM = BattleStateMachine.instance.HerosToManage[0].GetComponent<HeroStateMachine>();
        int index = HSM.hero.basicSkill.FindIndex(x => x.name.Equals(skillName));

        HeroChoice.Attacker= BattleStateMachine.instance.HerosToManage[0].name;
        HeroChoice.AttacksGameObject = HSM.gameObject;
        HeroChoice.Type = "Hero";
        HeroChoice.choosenAttack = HSM.hero.basicSkill[index];

        //HSM.UpdateAnim(skillName);

        // button panel SetActive(false)
        actionUI.SetUI(false);
    }

    // (2) hero input - enemy selection
    public void InputBasicSkill2(GameObject choosenEnemy)
    {
        if (HeroChoice.choosenAttack == null)
        {
            Debug.Log("skill select first!");
            return;
        }

        if (input == HeroInput.HeroGUI.DONE)
        {
            Debug.Log("hero input done!");
            return;
        }

        //BattleStateMachine.instance.HerosToManage[0].GetComponent<HeroStateMachine>().DoAttack(HeroChoice.choosenAttack);

        HeroChoice.AttackersTarget = choosenEnemy;
        //HeroStateMachine HSM = BattleStateMachine.instance.HerosToManage[0].GetComponent<HeroStateMachine>();
        StartCoroutine(DoBasicSkill(BattleStateMachine.instance.HerosToManage[0].GetComponent<HeroStateMachine>()));
        //input = HeroInput.HeroGUI.DONE;
    }
    #endregion

    // (4) Do special skill
    IEnumerator DoSpecialSkill()
    {
        HeroStateMachine HSM = BattleStateMachine.instance.HerosToManage[0].GetComponent<HeroStateMachine>();
        HSM.ShowSpecialSkillParticle();

        yield return new WaitForSeconds(2f);

        if (HeroChoice.choosenAttack.GetType() == typeof(Heal))
        {
            foreach (GameObject hero in BattleStateMachine.instance.HerosInBattle)
            {
                HSM.hero.curHP += (HSM.hero.curATK + HSM.hero.curDEF);
                if (HSM.hero.curHP > HSM.hero.baseHP)
                {
                    HSM.hero.curHP = HSM.hero.baseHP;
                }
                HSM.UpdateHeroBar();
            }
        }
        else if (HeroChoice.choosenAttack.GetType() == typeof(Snow))
        {
            float randNum = Random.Range(1, 6) * 0.1f;

            foreach (GameObject enemy in BattleStateMachine.instance.EnemysInBattle)
            {
                EnemyStateMachine ESM = enemy.GetComponent<EnemyStateMachine>();
                ESM.enemy.curDEF -= (ESM.enemy.baseDEF * randNum);
                if (ESM.enemy.curDEF < 0)
                {
                    ESM.enemy.curDEF = 0;
                }
            }
        }
        else
            Debug.Log("DoSpecialSkill ÀÌ»óÇÔ");
        input = HeroInput.HeroGUI.DONE;
    }

    IEnumerator DoBasicSkill(HeroStateMachine HSM)
    {
        yield return new WaitForSeconds(1f);
        GameObject obj = BattleStateMachine.instance.HerosToManage[0];
        obj.GetComponent<HeroStateMachine>().DoAttack(HeroChoice.choosenAttack);
        input = HeroInput.HeroGUI.DONE;
    }
    void HeroInputDone()
    {
        HeroStateMachine HSM = BattleStateMachine.instance.HerosToManage[0].GetComponent<HeroStateMachine>();

        HSM.UpdateAnim(HeroChoice.choosenAttack.name);

        BattleStateMachine.instance.PerformList.Add(HeroChoice);

        HSM.EnemyToAttack = HeroChoice.AttackersTarget;

        BattleStateMachine.instance.battleStates = BattleStateMachine.PerformAction.PERFORMACTION;

        actionUI.clearAttackPanel();
        BattleStateMachine.instance.HerosToManage.RemoveAt(0);
        this.gameObject.transform.Find("Selector").gameObject.SetActive(false);

        input = HeroInput.HeroGUI.WAITING;
    }
}
