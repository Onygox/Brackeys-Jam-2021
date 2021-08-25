using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //put movement and visual scripting in here

    [SerializeField] float baseSpeed = 2.7f;
    Vector3 velocity;
    private Rigidbody2D thisBody;
    public CircleCollider2D thisCollider;
    public GameObject lookTarget;
    public HealthComponent playerHealthComponent;

    void Start() {
        thisBody = GetComponent<Rigidbody2D>();
        thisCollider = GetComponentInChildren<CircleCollider2D>();
        playerHealthComponent = GetComponent<HealthComponent>();
        playerHealthComponent.healthSlider = GameManager.Instance.uiManager.playerHealthSlider;
        playerHealthComponent.MaxHealth = GameManager.Instance.playerManager.maxPlayerHealthVar.Value;
        GameManager.Instance.playerManager.currentPlayerHealthVar.Value = GameManager.Instance.playerManager.maxPlayerHealthVar.Value;
        playerHealthComponent.Health = GameManager.Instance.playerManager.currentPlayerHealthVar.Value;
    }

    void Update() {

        velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        thisBody.velocity = velocity * baseSpeed;

        lookTarget.transform.position = transform.position + velocity;

        if (Input.GetButtonDown("Fire Horizontal")) {
            ShootWeapon(new Vector3(Input.GetAxis("Fire Horizontal"), 0, 0));
        }

        if (Input.GetButtonDown("Fire Vertical")) {
            ShootWeapon(new Vector3(0, Input.GetAxis("Fire Vertical"), 0));
        }

        // if (Input.GetButtonDown("Reload")) {
        //     if (GameManager.Instance.playerManager.playerShootingBehaviour.reloadTime >= GameManager.Instance.playerManager.playerShootingBehaviour.currentWeapon.ReloadSpeed &&
        //         GameManager.Instance.playerManager.playerShootingBehaviour.currentWeapon.ClipSize < GameManager.Instance.playerManager.playerShootingBehaviour.currentWeapon.MaxClipSize) {
        //         StartCoroutine(GameManager.Instance.playerManager.playerShootingBehaviour.ReloadWeaponRoutine());
        //     }
        // }
    }

    private void ShootWeapon(Vector3 direction) {
        int startingZRotation = 0;

        switch (direction.x) {

            case 1:
                startingZRotation = 270;
                break;
            case -1:
                startingZRotation = 90;
                break;
            case 0:
                startingZRotation = direction.y > 0 ? 0 : 180;
                break;
            default:
                break;

        }

        GameManager.Instance.playerManager.playerShootingBehaviour.ShootWeapon(new Vector3(0, 0, startingZRotation));
        GameManager.Instance.uiManager.ammoSlider.value = GameManager.Instance.playerManager.playerShootingBehaviour.currentWeapon.ClipSize;
        GameManager.Instance.uiManager.ammoText.text = GameManager.Instance.playerManager.playerShootingBehaviour.currentWeapon.MaxClipSize > 0 ? "Ammo Left: " + GameManager.Instance.playerManager.playerShootingBehaviour.currentWeapon.ClipSize.ToString() + "/" + GameManager.Instance.playerManager.playerShootingBehaviour.currentWeapon.MaxClipSize.ToString() : "Ammo Left: ∞";
    }
}
