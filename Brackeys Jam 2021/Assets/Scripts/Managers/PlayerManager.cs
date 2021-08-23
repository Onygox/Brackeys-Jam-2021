using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public ScriptableInt currentPlayerHealthVar, maxPlayerHealthVar;
    public Weapon currentWeapon;

    void Start() {
        currentPlayerHealthVar = PersistentManager.Instance.FindVariableBySavePath("currentplayerhealthdata") as ScriptableInt;
        maxPlayerHealthVar = PersistentManager.Instance.FindVariableBySavePath("maximumplayerhealthdata") as ScriptableInt;
    }
}
