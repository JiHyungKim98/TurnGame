
using UnityEngine;

public class Spin : SkillInfo
{
    public Sprite img;
    public Spin()
    {
        SkillInfo spinSkill = new SkillInfo();
        spinSkill.skillName = "Spin";
        spinSkill.skillDescription = "Spin with attack";
        spinSkill.skillDamage = 20;
        spinSkill.skillCost = 10f;
        spinSkill.skillLimit = 0;
        spinSkill.skillImg = img;

        SkillManager.skills.Add(spinSkill);
    }

    public override void DoSkill()
    {
        base.DoSkill();
    }
    
    public bool isSuccess()
    {
        int n = Random.Range(0, 2);
        return n > 0 ? true : false;
    }
}
