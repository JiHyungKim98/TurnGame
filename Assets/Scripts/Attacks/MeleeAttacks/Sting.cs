using UnityEngine;
using System.Collections;

public class Sting : SkillInfo
{
    public Sprite img;
    public Sting()
    {
        SkillInfo stingSkill = new SkillInfo();
        stingSkill.skillName = "Sting";
        stingSkill.skillDescription = "Sting with attack";
        stingSkill.skillDamage = 20;
        stingSkill.skillCost = 10f;
        stingSkill.skillLimit = 0;
        stingSkill.skillImg = img;

        SkillManager.skills.Add(stingSkill);
    }

    public override void DoSkill()
    {
        base.DoSkill();
    }

    public bool isSuccess()
    {
        int n = Random.Range(0, 3);
        return n < 1 ? true : false;
    }
}
