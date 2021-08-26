using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationAura : MonoBehaviour
{
    public TerminalScript closestTerminal = null;
    
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.GetComponent<TerminalScript>()) {
            TerminalScript ts = collider.gameObject.GetComponent<TerminalScript>();
            if (!ts.hasBeenActivated) {
                collider.gameObject.GetComponent<TerminalScript>().activationUI.SetActive(true);
                closestTerminal = collider.gameObject.GetComponent<TerminalScript>();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.gameObject.GetComponent<TerminalScript>()) {
            collider.gameObject.GetComponent<TerminalScript>().activationUI.SetActive(false);
            closestTerminal = null;
        }
    }
}
