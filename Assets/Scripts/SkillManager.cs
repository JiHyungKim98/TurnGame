using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager
{
    //public static SkillManager instance = null;

    public static List<SkillInfo> skills=new List<SkillInfo>();

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        if (instance != this)
    //        {
    //            Destroy(this.gameObject);
    //        }
    //    }
    //}


    public SkillInfo getSkillInfo(SkillInfo skill)
    {
        int index = skills.FindIndex(x => x.name.Equals(skill.name));
        return skills[index];
    }
    public void addSkillInfo(SkillInfo skill)
    {
        if (skills.Contains(skill))
        {
            return;
        }
        skills.Add(skill);
    }
}

