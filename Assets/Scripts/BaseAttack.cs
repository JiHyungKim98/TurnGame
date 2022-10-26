using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BaseAttack : MonoBehaviour
{
    public string attackName;
    public string attackDiscription; // explain
    public float attackDamage; // Base Damage 15, mellee lv10 stemina 35 = base Damage + lv + stemina
    public float attackCost; //Mana Cost

    public Sprite attackImg;
    public int turnLimit;
}
