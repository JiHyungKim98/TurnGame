using System;
using UnityEngine;
public class Snow : SkillInfo
{
    public Sprite img;
    public Snow()
    {
        SkillInfo snowSkill = new SkillInfo();
        snowSkill.skillName = "Snow";
        snowSkill.skillDescription = "Decrease enemy's defense with a chance between 10% and 50%";
        snowSkill.skillDamage = 0;
        snowSkill.skillCost = 0f;
        snowSkill.skillLimit = 0;
        snowSkill.skillImg = img;

        SkillManager.skills.Add(snowSkill);
    }
    public override void DoSkill()
    {
        base.DoSkill();
    }
}
