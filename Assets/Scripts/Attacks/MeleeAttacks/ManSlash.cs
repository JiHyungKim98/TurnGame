using System;
using UnityEngine;
public class ManSlash : SkillInfo
{
    public Sprite img;
    public ManSlash()
    {
        SkillInfo slashSkill = new SkillInfo();
        slashSkill.skillName = "ManSlash";
        slashSkill.skillDescription = "Fast Slash attack to the enemy vertically";
        slashSkill.skillDamage = 10;
        slashSkill.skillCost = 0f;
        slashSkill.skillLimit = 0;
        slashSkill.skillImg = img;

        SkillManager.skills.Add(slashSkill);
    }
    public override void DoSkill()
    {
        base.DoSkill();
    }
}
