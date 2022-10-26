using System;
using UnityEngine;

public class WomanSlash : SkillInfo
{
    public Sprite img;
    public WomanSlash()
    {
        SkillInfo slashSkill = new SkillInfo();
        slashSkill.skillName = "WomanSlash";
        slashSkill.skillDescription = "Fast Slash attack to the enemy horizontally";
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
