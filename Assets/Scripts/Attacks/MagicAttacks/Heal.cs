using UnityEngine;

public class Heal : SkillInfo
{
    public Sprite img;
    public Heal()
    {
        SkillInfo healSkill = new SkillInfo();
        healSkill.skillName = "Heal";
        healSkill.skillDescription = "Decrease enemy's defense with a chance between 10% and 50%";
        healSkill.skillDamage = 0;
        healSkill.skillCost = 0f;
        healSkill.skillLimit = 0;
        healSkill.skillImg = img;

        SkillManager.skills.Add(healSkill);
    }
    public override void DoSkill()
    {
        base.DoSkill();
    }
}
