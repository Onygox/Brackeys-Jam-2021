using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //put movement and visual scripting in here

    [SerializeField] float baseSpeed = 2.7f;
    Vector3 velocity;
    private Rigidbody2D thisBody;
    [SerializeField] private Animator torsoAnim, legsAnim;
    public CircleCollider2D thisCollider;
    public GameObject lookTarget;
    public HealthComponent playerHealthComponent;
    public ShootingBehaviour playerShootingBehaviour;
    public SpriteRenderer gunSprite;
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

        StartCoroutine(PlayFootstepNoises());
    }

    void Update() {

        if (GameManager.Instance.GameIsOver()) return;

        velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        if (legsAnim) {
            legsAnim.SetFloat("xVelFloat", Input.GetAxis("Horizontal"));
            legsAnim.SetFloat("yVelFloat", Input.GetAxis("Vertical"));
        }
        // if (torsoAnim) {
        //     torsoAnim.SetFloat("xDir", Input.GetAxis("Horizontal"));
        //     torsoAnim.SetFloat("yDir", Input.GetAxis("Vertical"));
        // }

        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0) {
            torsoAnim.SetFloat("yDir", 0);
            torsoAnim.SetFloat("xDir", Input.GetAxis("Horizontal"));
        } else if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0) {
            torsoAnim.SetFloat("yDir", Input.GetAxis("Vertical"));
            torsoAnim.SetFloat("xDir", 0);
        }

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

        if (torsoAnim) {
            torsoAnim.SetFloat("xDir", direction.x);
            torsoAnim.SetFloat("yDir", direction.y);
        }

        int startingZRotation = 0;

        switch (direction.x) {

            case 1:
                // if (playerShootingBehaviour.currentWeapon.weaponImages.Length > 0) gunSprite.sprite = playerShootingBehaviour.currentWeapon.weaponImages[0];
                startingZRotation = 270;
                break;
            case -1:
                // if (playerShootingBehaviour.currentWeapon.weaponImages.Length > 0) gunSprite.sprite = playerShootingBehaviour.currentWeapon.weaponImages[1];
                startingZRotation = 90;
                break;
            case 0:
                startingZRotation = direction.y > 0 ? 0 : 180;
                // if (playerShootingBehaviour.currentWeapon.weaponImages.Length > 0) gunSprite.sprite = direction.y > 0 ? playerShootingBehaviour.currentWeapon.weaponImages[2] : playerShootingBehaviour.currentWeapon.weaponImages[3];
                break;
            default:
                break;

        }

        if (torsoAnim) torsoAnim.SetTrigger("Shoot");

        if (playerShootingBehaviour.timeSinceLastShot >= playerShootingBehaviour.currentWeapon.FireRate && playerShootingBehaviour.currentWeapon.Recoil > 0) {
            StartCoroutine(RecoilRoutine(-direction, playerShootingBehaviour.currentWeapon.Recoil));
        }

        playerShootingBehaviour.ShootWeapon(transform.position + (direction*0.67f), new Vector3(0, 0, startingZRotation));

        GameManager.Instance.uiManager.ammoSlider.value = playerShootingBehaviour.currentWeapon.ClipSize;
        GameManager.Instance.uiManager.ammoText.text = playerShootingBehaviour.currentWeapon.MaxClipSize > 0 ? "Ammo Left: " + playerShootingBehaviour.currentWeapon.ClipSize.ToString() + "/" + playerShootingBehaviour.currentWeapon.MaxClipSize.ToString() : "Ammo Left: ∞";
    }

    public void OnWeaponChange(string animTrigger) {
        if (torsoAnim) torsoAnim.SetTrigger(animTrigger);
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

    public IEnumerator PlayFootstepNoises() {
        while (true) {
            yield return new WaitForSeconds(0.25f * GameManager.Instance.playerManager.playerMovementSpeedVar.Value);
            if (thisBody.velocity.x != 0 || thisBody.velocity.y != 0) {
                PersistentManager.Instance.soundManager.PlayRandomPlayerFootstepSound();
            }
        }
    }

}
