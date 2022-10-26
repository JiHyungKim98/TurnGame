using UnityEngine;
using System;

[Serializable]
public class HandleTurn
{
    public string Attacker; // name of attacker
    public string Type;
    public GameObject AttacksGameObject; // who attacks
    public GameObject AttackersTarget; // who is going to be attacked

    public SkillInfo choosenAttack; 
}
