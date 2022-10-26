using System;
using System.Collections.Generic;

[Serializable]
public class BaseHero : BaseClass
{
    public int stemina;
    public int intellect; 
    public int dexterity;
    public int agility; 

    public List<SkillInfo> SpecialSkill = new List<SkillInfo>();
}
