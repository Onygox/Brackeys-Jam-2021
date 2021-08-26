using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public ScriptableInt currentPlayerHealthVar, maxPlayerHealthVar;
    public ScriptableFloat playerMovementSpeedVar;
    [HideInInspector] public PlayerScript playerScript;

    void Start() {
        currentPlayerHealthVar = PersistentManager.Instance.FindVariableBySavePath("currentplayerhealthdata") as ScriptableInt;
        maxPlayerHealthVar = PersistentManager.Instance.FindVariableBySavePath("maximumplayerhealthdata") as ScriptableInt;
    }

    public void ChangeWeapon(Weapon newWeapon) {
        playerScript.playerShootingBehaviour.ChangeWeapons(newWeapon);
        playerScript.playerShootingBehaviour.ReloadWeapon();
    }

    public void ChangeWeaponByLibraryIndex(int libraryIndex) {
        playerScript.playerShootingBehaviour.ChangeWeapons(PersistentManager.Instance.weaponLibrary[libraryIndex]);
        playerScript.playerShootingBehaviour.ReloadWeapon();
    }

    public void OnWeaponReload() {
        GameManager.Instance.uiManager.ammoSlider.maxValue = playerScript.playerShootingBehaviour.currentWeapon.MaxClipSize;
        GameManager.Instance.uiManager.ammoSlider.value = playerScript.playerShootingBehaviour.currentWeapon.ClipSize;
        GameManager.Instance.uiManager.ammoText.text = playerScript.playerShootingBehaviour.currentWeapon.MaxClipSize > 0 ? "Ammo Left: " + playerScript.playerShootingBehaviour.currentWeapon.ClipSize.ToString() + "/" + playerScript.playerShootingBehaviour.currentWeapon.MaxClipSize.ToString() : "Ammo Left: ∞";
    }
}
