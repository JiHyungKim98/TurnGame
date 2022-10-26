using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lateSkillCount : SkillManager
{
    public int CheckSkillLimit(string choosenAttack)
    {
        int index = skills.FindIndex(x => x.skillName.Equals(choosenAttack));
        return skills[index].skillLimit;
    }

    public void setSkillLimit(string choosenAttack)
    {
        int index = skills.FindIndex(x => x.skillName.Equals(choosenAttack));

        switch (choosenAttack)
        {
            case "Sting":
            case "Spin":
                if (skills[index].skillLimit == 0)
                    skills[index].skillLimit = 2;
                break;

            case "Heal":
            case "Snow":
                if (skills[index].skillLimit == 0)
                    skills[index].skillLimit = 3;
                break;

            default:
                return;
        }
    }
    public void skillLimitDesc()
    {
        foreach(var skill in skills)
        {
            if (skill.skillLimit > 0)
                skill.skillLimit--;
        }
    }
}
