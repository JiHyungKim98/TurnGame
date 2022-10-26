using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class UpdateUI : MonoBehaviour
{
    public TextMeshProUGUI curHP;
    public TextMeshProUGUI curMP;
    public TextMeshProUGUI curAtk;
    public TextMeshProUGUI curDef;

    public GameObject obj;
    HeroStateMachine HSM;
    EnemyStateMachine ESM;

    private void Start()
    {
        HSM = obj.GetComponent<HeroStateMachine>();
        ESM = obj.GetComponent<EnemyStateMachine>();
    }
    private void Update()
    {
        if (obj.tag == "Hero")
        {
            curHP.text = HSM.hero.curHP.ToString();
            curMP.text = HSM.hero.curMP.ToString();
            curAtk.text = HSM.hero.curATK.ToString();
            curDef.text = HSM.hero.curDEF.ToString();
        }
        else
        {
            curHP.text = ESM.enemy.curHP.ToString();
            curMP.text = ESM.enemy.curMP.ToString();
            curAtk.text = ESM.enemy.curATK.ToString();
            curDef.text = ESM.enemy.curDEF.ToString();
        }
        
    }
}
