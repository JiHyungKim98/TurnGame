using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo : MonoBehaviour
{
    public string skillName;
    public string skillDescription;

    public float skillDamage;
    public float skillCost;

    public Sprite skillImg;

    public int skillLimit;

    public virtual void DoSkill() { }
}
