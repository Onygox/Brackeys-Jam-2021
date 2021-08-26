using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBehaviour : MonoBehaviour
{
    public float timeSinceLastShot = 0;
    public float reloadTime = 0;
    public Weapon currentWeapon;
    private bool isPlayer;
    public ScriptableFloat playerFireRateMultiplier;
    // public bool isReloading;

    void Start() {
        isPlayer = GetComponent<PlayerScript>() != null;
        ReloadWeapon();
    }

    public void ShootWeapon(Vector3 location, Vector3 direction) {

        if (timeSinceLastShot < currentWeapon.FireRate) {
            return;
        }

        if (reloadTime < currentWeapon.ReloadSpeed) {
            return;
        }

        if (currentWeapon.ClipSize <= 0 && currentWeapon.MaxClipSize > 0) {
            return;
        }

        StopCoroutine("CoolDown");

        currentWeapon.Shoot(location, direction, this.gameObject);

        if (isPlayer && currentWeapon.ClipSize <= 0) {
            //change weapon automatically on clip empty
            ChangeWeapons(PersistentManager.Instance.weaponLibrary[Mathf.FloorToInt(Random.Range(0, PersistentManager.Instance.weaponLibrary.Length))]);
        }

        StartCoroutine("CoolDown");
        
    }

    IEnumerator CoolDown() {
        timeSinceLastShot = 0;
        GameManager.Instance.uiManager.readyToFireText.text = " ";
        while (timeSinceLastShot <= currentWeapon.FireRate) {
            yield return new WaitForSeconds(0.1f);
            if (isPlayer) {
                timeSinceLastShot += (0.1f * playerFireRateMultiplier.Value);
                GameManager.Instance.uiManager.fireRateSlider.value = ExtensionMethods.Remap(timeSinceLastShot, 0.0f, currentWeapon.FireRate, 0.0f, 1.0f);
            } else {
                timeSinceLastShot += 0.1f;
            }
        }
        GameManager.Instance.uiManager.readyToFireText.text = "Ready To Fire";
    }

    // public IEnumerator ReloadWeaponRoutine() {
    //     reloadTime = 0;
    //     GameManager.Instance.uiManager.ammoText.text = "Reloading";
    //     while (reloadTime <= currentWeapon.ReloadSpeed) {
    //         yield return new WaitForSeconds(0.1f);
    //         reloadTime += 0.1f;
    //         if (isPlayer) GameManager.Instance.uiManager.reloadTimeSlider.value = ExtensionMethods.Remap(reloadTime, 0.0f, currentWeapon.ReloadSpeed, 0.0f, 1.0f);
    //     }
    //     ReloadWeapon();
    // }

    public void ChangeWeapons(Weapon newWeapon) {
        currentWeapon = newWeapon;
        GameManager.Instance.uiManager.currentWeaponText.text = "Current Weapon: " + currentWeapon.name;
        ReloadWeapon();
    }

    public void ReloadWeapon() {
        currentWeapon.Reload();
        timeSinceLastShot = Mathf.CeilToInt(currentWeapon.FireRate);
        reloadTime = Mathf.CeilToInt(currentWeapon.ReloadSpeed);
        if (isPlayer) GameManager.Instance.playerManager.OnWeaponReload();
    }
}
