using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceButtonScript : MonoBehaviour
{
    [HideInInspector] public TerminalEffect thisEffect;

    public void PerformThisEffect() {
        Time.timeScale = 1;
        thisEffect.PerformEffect();
        GameManager.Instance.uiManager.SendFleetingMessage(GameManager.Instance.playerManager.playerScript.gameObject.transform.position + new Vector3(0, 1, 0), thisEffect.message);
        GameManager.Instance.NumberOfActiveTerminals++;
    }

    public void ChangeEffect(TerminalEffect newEffect) {
        thisEffect = newEffect;
    }
}
