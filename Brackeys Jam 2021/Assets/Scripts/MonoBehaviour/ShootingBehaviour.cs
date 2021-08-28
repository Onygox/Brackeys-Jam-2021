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
    private AudioPlayer sounds;
    private AudioSource weaponAudio;

    private void Awake()
    {
        weaponAudio = gameObject.AddComponent<AudioSource>();
        sounds = GetComponent<AudioPlayer>();
    }

    void Start() {
        isPlayer = GetComponent<PlayerScript>() != null;
        ReloadWeapon();
        SetupWeaponAudio();
    }

    public void ShootWeapon(Vector3 location, Vector3 direction) {

        if (timeSinceLastShot < currentWeapon.FireRate) {
            return;
        }

        if (reloadTime < currentWeapon.ReloadSpeed) {
            return;
        }

        if (currentWeapon.ClipSize <= 0 && currentWeapon.MaxClipSize > 0) {
            sounds.Play("Out Of Ammo");
            return;
        }

        StopCoroutine("CoolDown");

        currentWeapon.Shoot(location, direction, this.gameObject);
        AudioPlayer.PlaySFX(currentWeapon.sound);
        // todo: last bullet of clip won't play sound because it gets swapped out
        // but this will likely be fixed if we bind swap weapon to a button instead of auto swapping

        if (isPlayer && currentWeapon.ClipSize <= 0) {
            //change weapon automatically on clip empty
            ChangeWeapons(PersistentManager.Instance.weaponLibrary[Mathf.FloorToInt(Random.Range(0, PersistentManager.Instance.weaponLibrary.Length))]);
        }

        StartCoroutine("CoolDown");
        
    }

    IEnumerator CoolDown() {
        timeSinceLastShot = 0;
        if (GameManager.Instance.uiManager.readyToFireText) GameManager.Instance.uiManager.readyToFireText.text = " ";
        while (timeSinceLastShot <= currentWeapon.FireRate) {
            yield return new WaitForSeconds(0.1f);
            if (isPlayer) {
                timeSinceLastShot += (0.1f * playerFireRateMultiplier.Value);
                GameManager.Instance.uiManager.fireRateSlider.value = ExtensionMethods.Remap(timeSinceLastShot, 0.0f, currentWeapon.FireRate, 0.0f, 1.0f);
            } else {
                timeSinceLastShot += 0.1f;
            }
        }
        if (GameManager.Instance.uiManager.readyToFireText) GameManager.Instance.uiManager.readyToFireText.text = "Ready To Fire";
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

    private void SetupWeaponAudio()
    {
        if (currentWeapon != null)
            AudioPlayer.SetUpSound(currentWeapon.sound, weaponAudio);
    }

    public void ChangeWeapons(Weapon newWeapon) {
        currentWeapon = newWeapon;
        SetupWeaponAudio();
        sounds.Play("Change Weapon");
        if (GameManager.Instance.uiManager.currentWeaponText) GameManager.Instance.uiManager.currentWeaponText.text = "Current Weapon: " + currentWeapon.name;
        ReloadWeapon();
        if (isPlayer) GameManager.Instance.playerManager.playerScript.OnWeaponChange(currentWeapon.animTrigger);
    }

    public void ReloadWeapon() {
        currentWeapon.Reload();
        timeSinceLastShot = Mathf.CeilToInt(currentWeapon.FireRate);
        reloadTime = Mathf.CeilToInt(currentWeapon.ReloadSpeed);
        if (isPlayer) {
            GameManager.Instance.playerManager.OnWeaponReload();
            if (GameManager.Instance.playerManager.playerScript.playerShootingBehaviour.currentWeapon.weaponImages.Length > 0) GameManager.Instance.playerManager.playerScript.gunSprite.sprite = GameManager.Instance.playerManager.playerScript.playerShootingBehaviour.currentWeapon.weaponImages[0];
        }
    }
}
