﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBehaviour : MonoBehaviour
{
    private float timeSinceLastShot = 0;
    public float reloadTime = 0;
    public Weapon currentWeapon;
    private bool isPlayer;
    // public bool isReloading;

    void Start() {
        isPlayer = GetComponent<PlayerScript>() != null;
        ReloadWeapon();
    }

    public void ShootWeapon(Vector3 direction) {

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

        currentWeapon.Shoot(transform.position + direction/2, direction, this.gameObject);

        StartCoroutine("CoolDown");
        
    }

    IEnumerator CoolDown() {
        timeSinceLastShot = 0;
        while (timeSinceLastShot <= currentWeapon.FireRate) {
            yield return new WaitForSeconds(0.1f);
            timeSinceLastShot += 0.1f;
            if (isPlayer) GameManager.Instance.uiManager.fireRateSlider.value = ExtensionMethods.Remap(timeSinceLastShot, 0.0f, currentWeapon.FireRate, 0.0f, 1.0f);
        }
    }

    public IEnumerator ReloadWeaponRoutine() {
        reloadTime = 0;
        GameManager.Instance.uiManager.ammoText.text = "Reloading";
        while (reloadTime <= currentWeapon.ReloadSpeed) {
            yield return new WaitForSeconds(0.1f);
            reloadTime += 0.1f;
            if (isPlayer) GameManager.Instance.uiManager.reloadTimeSlider.value = ExtensionMethods.Remap(reloadTime, 0.0f, currentWeapon.ReloadSpeed, 0.0f, 1.0f);
        }
        ReloadWeapon();
    }

    public void ChangeWeapons(Weapon newWeapon) {
        currentWeapon = newWeapon;
    }

    public void ReloadWeapon() {
        currentWeapon.Reload();
        timeSinceLastShot = Mathf.CeilToInt(currentWeapon.FireRate);
        reloadTime = Mathf.CeilToInt(currentWeapon.ReloadSpeed);
        if (isPlayer) GameManager.Instance.playerManager.OnWeaponReload();
    }
}