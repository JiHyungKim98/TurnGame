using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heroAutoInput : MonoBehaviour
{
    public Toggle toggleBtn;

    private void Awake()
    {
        toggleBtn.onValueChanged.AddListener(isAuto);
    }
    public void isAuto(bool isAuto)
    {
        if (!isAuto)
            BattleStateMachine.instance.isAuto = false;

        else
            BattleStateMachine.instance.isAuto = true;
    }
    
}
