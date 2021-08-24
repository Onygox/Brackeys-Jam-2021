using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public ScriptableInt currentPlayerHealthVar, maxPlayerHealthVar;
    [HideInInspector] public PlayerScript playerScript;
    [HideInInspector] public ShootingBehaviour playerShootingBehaviour;

    void Start() {
        currentPlayerHealthVar = PersistentManager.Instance.FindVariableBySavePath("currentplayerhealthdata") as ScriptableInt;
        maxPlayerHealthVar = PersistentManager.Instance.FindVariableBySavePath("maximumplayerhealthdata") as ScriptableInt;
    }

    public void ChangeWeapon(Weapon newWeapon) {
        playerShootingBehaviour.ChangeWeapons(newWeapon);
        playerShootingBehaviour.ReloadWeapon();
    }

    public void ChangeWeaponByLibraryIndex(int libraryIndex) {
        playerShootingBehaviour.ChangeWeapons(PersistentManager.Instance.weaponLibrary[libraryIndex]);
        playerShootingBehaviour.ReloadWeapon();
    }

    public void OnWeaponReload() {
        GameManager.Instance.uiManager.ammoSlider.maxValue = playerShootingBehaviour.currentWeapon.MaxClipSize;
        GameManager.Instance.uiManager.ammoSlider.value = playerShootingBehaviour.currentWeapon.ClipSize;
        GameManager.Instance.uiManager.ammoText.text = "Ammo Left: " + playerShootingBehaviour.currentWeapon.ClipSize.ToString() + "/" + playerShootingBehaviour.currentWeapon.MaxClipSize.ToString();
    }
}
