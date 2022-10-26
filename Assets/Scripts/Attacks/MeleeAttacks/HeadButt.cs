using UnityEngine;
using System.Collections;

public class HeadButt : SkillInfo
{
    public Sprite img;
    public HeadButt()
    {
        SkillInfo snowSkill = new SkillInfo();
        
        snowSkill.skillName = "HeadButt";
        snowSkill.skillDescription = "Basic Attack to the hero";
        snowSkill.skillDamage = 5;
        snowSkill.skillCost = 10f;
        snowSkill.skillLimit = 0;
        snowSkill.skillImg = img;

        SkillManager skillManager = new SkillManager();
        skillManager.addSkillInfo(snowSkill);
    }

    public override void DoSkill()
    {
        base.DoSkill();
    }
}
