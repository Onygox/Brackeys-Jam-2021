using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalScript : Obstacle {
    public GameObject activationUI;
    public bool hasBeenActivated = false;

    public void Activate() {
        if (!hasBeenActivated) {
            TerminalEffect newEffect = PersistentManager.Instance.terminalEffectLibrary[Random.Range(0, Mathf.FloorToInt(PersistentManager.Instance.terminalEffectLibrary.Length))];
            newEffect.PerformEffect();
            GameManager.Instance.uiManager.SendFleetingMessage(transform.position + new Vector3(0, 1, 0), newEffect.message);
            hasBeenActivated = true;
            activationUI.SetActive(false);
        }
    }
}
