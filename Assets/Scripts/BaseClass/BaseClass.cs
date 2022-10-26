using System.Collections.Generic;

public class BaseClass 
{
    public string name;

    public float baseHP;
    public float curHP;

    public float baseMP;
    public float curMP;

    public float baseATK; // attack
    public float curATK;

    public float baseDEF; // defense
    public float curDEF;

    public List<SkillInfo> basicSkill = new List<SkillInfo>();
}
