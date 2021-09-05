using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalScript : Obstacle {
    public GameObject activationUI;
    public Sprite activatedSprite;
    public bool hasBeenActivated = false;
    private AudioPlayer player;

    private void Start() {
        player = GetComponent<AudioPlayer>();
    }

    public void Activate() {
        if (!hasBeenActivated) {
            // TerminalEffect newEffect = PersistentManager.Instance.terminalEffectLibrary[Random.Range(0, Mathf.FloorToInt(PersistentManager.Instance.terminalEffectLibrary.Length))];
            // newEffect.PerformEffect();
            player.Play("Activate");
            // GameManager.Instance.uiManager.SendFleetingMessage(transform.position + new Vector3(0, 1, 0), "Terminal activated!\n" + newEffect.message);
            hasBeenActivated = true;
            activationUI.SetActive(false);
            // GameManager.Instance.NumberOfActiveTerminals++;
            GetComponentInChildren<SpriteRenderer>().sprite = activatedSprite;
            GameManager.Instance.uiManager.DisplayChoices();
        }
    }
}
