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
    public ShootingBehaviour playerShootingBehaviour;
    bool isRecoiling;
    ActivationAura aAura;

    void Start() {
        thisBody = GetComponent<Rigidbody2D>();
        thisCollider = GetComponentInChildren<CircleCollider2D>();
        playerHealthComponent = GetComponent<HealthComponent>();
        playerShootingBehaviour = GetComponent<ShootingBehaviour>();
        aAura = GetComponentInChildren<ActivationAura>();
        playerHealthComponent.healthSlider = GameManager.Instance.uiManager.playerHealthSlider;
        playerHealthComponent.MaxHealth = GameManager.Instance.playerManager.maxPlayerHealthVar.Value;
        GameManager.Instance.playerManager.currentPlayerHealthVar.Value = GameManager.Instance.playerManager.maxPlayerHealthVar.Value;
        playerHealthComponent.Health = GameManager.Instance.playerManager.currentPlayerHealthVar.Value;
    }

    void Update() {

        if (GameManager.Instance.GameIsOver()) return;

        velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        if (!isRecoiling) thisBody.velocity = velocity * baseSpeed * GameManager.Instance.playerManager.playerMovementSpeedVar.Value;

        lookTarget.transform.position = transform.position + velocity;

        if (playerShootingBehaviour.currentWeapon.automatic) {
            if (Input.GetButton("Fire Horizontal")) {
                ShootWeapon(new Vector3(Input.GetAxis("Fire Horizontal"), 0, 0));
            }

            if (Input.GetButton("Fire Vertical")) {
                ShootWeapon(new Vector3(0, Input.GetAxis("Fire Vertical"), 0));
            }
        } else {
            if (Input.GetButtonDown("Fire Horizontal")) {
                ShootWeapon(new Vector3(Input.GetAxis("Fire Horizontal"), 0, 0));
            }

            if (Input.GetButtonDown("Fire Vertical")) {
                ShootWeapon(new Vector3(0, Input.GetAxis("Fire Vertical"), 0));
            }
        }

        if (Input.GetButtonDown("Activate")) {
            if (aAura.closestTerminal != null && !aAura.closestTerminal.hasBeenActivated) {
                aAura.closestTerminal.Activate();
            }
        }
        
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

        if (playerShootingBehaviour.timeSinceLastShot >= playerShootingBehaviour.currentWeapon.FireRate && playerShootingBehaviour.currentWeapon.Recoil > 0) {
            StartCoroutine(RecoilRoutine(-direction, playerShootingBehaviour.currentWeapon.Recoil));
        }

        playerShootingBehaviour.ShootWeapon(transform.position + (direction*0.67f), new Vector3(0, 0, startingZRotation));

        GameManager.Instance.uiManager.ammoSlider.value = playerShootingBehaviour.currentWeapon.ClipSize;
        GameManager.Instance.uiManager.ammoText.text = playerShootingBehaviour.currentWeapon.MaxClipSize > 0 ? "Ammo Left: " + playerShootingBehaviour.currentWeapon.ClipSize.ToString() + "/" + playerShootingBehaviour.currentWeapon.MaxClipSize.ToString() : "Ammo Left: ∞";
    }

    public IEnumerator RecoilRoutine(Vector2 direction, float strength, float delay = 0.7f) {
        isRecoiling = true;
        thisBody.AddForce(direction*strength, ForceMode2D.Impulse);
        yield return new WaitForSeconds(delay);
        isRecoiling = false;
    }

    public void GetKnockedBack(Vector2 direction, float strength, float delay = 0.7f) {
        StopCoroutine(RecoilRoutine(direction, strength, delay));
        StartCoroutine(RecoilRoutine(direction, strength, delay));
    }

}
