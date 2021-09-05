using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Brackeys Jam/Terminal Effect")]
public class TerminalEffect : ScriptableObject {

    public ScriptableFloat floatToAffect;
    public ScriptableInt intToAffect;
    public float floatAmount;
    public int intAmount;
    public string message;
    public string description;

    public void PerformEffect() {

        if (floatToAffect != null) {
            floatToAffect.Value += floatAmount;
        }

        if (intToAffect != null) {
            intToAffect.Value += intAmount;
        }

        Debug.Log(message);

    }
}
