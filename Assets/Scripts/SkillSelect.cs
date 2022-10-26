using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class SkillSelect : MonoBehaviour
{
    public Button btn;
    HeroInput heroInput;
    lateSkillCount skillCount=new lateSkillCount();

    private void Start()
    {
        btn.onClick.AddListener(input);
        heroInput = BattleStateMachine.instance.HerosToManage[0].GetComponent<HeroInput>();
        //GameObject.Find("BattleManager").GetComponent<HeroInput>();
    }
    void input()
    {
        heroInput.InputBasicSkill(this.gameObject.name);
        skillCount.setSkillLimit(btn.name);
    }
}
