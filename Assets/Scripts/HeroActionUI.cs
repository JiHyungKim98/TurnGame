using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroActionUI : MonoBehaviour
{
    public GameObject actionPanel;

    // attack of heros
    public GameObject basicSkillBtn;
    public GameObject specialSkillBtn;
    private List<GameObject> atkBtns = new List<GameObject>();

    // Skill Late 
    lateSkillCount skillcnt = new lateSkillCount();


    private void Awake()
    {
        actionPanel.SetActive(false);
        Debug.Log("µé¾î¿È");
    }
    public void SetUI(bool isActive)
    {
        //Debug.Log("µé¾î¿È" + isActive);
        if (isActive)
            actionPanel.SetActive(true);
        else
            actionPanel.SetActive(false);
    }

    // create action buttons
    public void CreateAttackButtons()
    {
        if (atkBtns.Count == BattleStateMachine.instance.HerosInBattle[0].GetComponent<HeroStateMachine>().hero.basicSkill.Count + 1)
            return;

        if (BattleStateMachine.instance.turn == BattleStateMachine.WhosTurn.ENEMY)
        {
            clearAttackPanel();
        }
        HeroStateMachine HSM = BattleStateMachine.instance.HerosToManage[0].GetComponent<HeroStateMachine>();
        // basic skills
        for (int i = 0; i < HSM.hero.basicSkill.Count; i++)
        {
            GameObject AttackButton = Instantiate(basicSkillBtn) as GameObject;
            Image AttackButtonImg = AttackButton.transform.Find("Image").gameObject.GetComponent<Image>();

            AttackButtonImg.sprite = HSM.hero.basicSkill[i].skillImg;
            AttackButton.name = HSM.hero.basicSkill[i].name;

            AttackButton.GetComponentInChildren<TextMeshProUGUI>().text = skillcnt.CheckSkillLimit(AttackButton.name).ToString();

            CheckLimit(AttackButton);

            atkBtns.Add(AttackButton);
        }

        // special skills
        GameObject SpecialAttackButton = Instantiate(specialSkillBtn) as GameObject;
        Image SpecialAttackButtonImg = SpecialAttackButton.transform.Find("Image").gameObject.GetComponent<Image>();

        SpecialAttackButtonImg.sprite = HSM.hero.SpecialSkill[0].skillImg;
        SpecialAttackButton.name = HSM.hero.SpecialSkill[0].name;

        SpecialAttackButton.GetComponentInChildren<TextMeshProUGUI>().text = skillcnt.CheckSkillLimit(SpecialAttackButton.name).ToString();

        CheckLimit(SpecialAttackButton);

        atkBtns.Add(SpecialAttackButton);
    }
    public void CheckLimit(GameObject btn)
    {
        // check limit count
        if (skillcnt.CheckSkillLimit(btn.name) > 0)
        {
            btn.GetComponent<Button>().interactable = false;
        }
        else
        {
            btn.GetComponent<Button>().interactable = true;
            btn.GetComponentInChildren<TextMeshProUGUI>().text = null;
        }
        btn.transform.SetParent(actionPanel.transform, false);
    }

    public void clearAttackPanel()
    {
        actionPanel.SetActive(false);
        foreach (GameObject atkBtn in atkBtns)
        {
            Destroy(atkBtn);
        }
        atkBtns.Clear();
    }
}
