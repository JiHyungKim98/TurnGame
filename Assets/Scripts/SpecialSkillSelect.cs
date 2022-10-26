using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialSkillSelect : MonoBehaviour
{
    public Button btn;
    HeroInput heroInput;
    lateSkillCount skillCount = new lateSkillCount();

    private void Start()
    {
        btn.onClick.AddListener(input);
        heroInput = BattleStateMachine.instance.HerosToManage[0].GetComponent<HeroInput>();
    }
    void input()
    {
        heroInput.InputSpecialSkill(btn.name);
        skillCount.setSkillLimit(btn.name);
    }
}
