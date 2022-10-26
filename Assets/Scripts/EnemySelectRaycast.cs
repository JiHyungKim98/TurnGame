using System.Collections;
using UnityEngine;
using System;

public class EnemySelectRaycast : MonoBehaviour
{
    private void OnMouseDown()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        if (hit.collider != null)
        {
            SelectEnemy();
        }

    }
    public void SelectEnemy()
    {
        BattleStateMachine.instance.HerosToManage[0].GetComponent<HeroInput>().InputBasicSkill2(this.gameObject);
    }
    //GameObject.Find("BattleManager").GetComponent<HeroInput>().InputBasicSkill2(this.gameObject);

}
