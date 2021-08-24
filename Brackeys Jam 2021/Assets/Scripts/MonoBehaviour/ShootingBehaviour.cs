using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBehaviour : MonoBehaviour
{
    private int timeSinceLastShot = 0;
    [SerializeField] private Weapon currentWeapon;

    void Start() {
        timeSinceLastShot = Mathf.CeilToInt(currentWeapon.FireRate);
    }

    public void ShootWeapon(Vector3 direction) {

        if (timeSinceLastShot < currentWeapon.FireRate) {
            return;
        } else {

            StopCoroutine("CoolDown");

            currentWeapon.Shoot(transform.position + direction/2, direction, this.gameObject);

            StartCoroutine("CoolDown");
        }
    }

    IEnumerator CoolDown() {
        timeSinceLastShot = 0;
        while (timeSinceLastShot <= currentWeapon.FireRate) {
            yield return new WaitForSeconds(1);
            timeSinceLastShot++;
        }
    }

    public void ChangeWeapons(Weapon newWeapon) {
        currentWeapon = newWeapon;
    }

    public void ReloadWeapon() {
        currentWeapon.Reload();
    }
}
