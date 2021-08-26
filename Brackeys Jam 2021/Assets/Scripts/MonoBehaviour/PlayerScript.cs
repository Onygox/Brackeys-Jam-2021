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

    void Start() {
        thisBody = GetComponent<Rigidbody2D>();
        thisCollider = GetComponentInChildren<CircleCollider2D>();
        playerHealthComponent = GetComponent<HealthComponent>();
        playerShootingBehaviour = GetComponent<ShootingBehaviour>();
        playerHealthComponent.healthSlider = GameManager.Instance.uiManager.playerHealthSlider;
        playerHealthComponent.MaxHealth = GameManager.Instance.playerManager.maxPlayerHealthVar.Value;
        GameManager.Instance.playerManager.currentPlayerHealthVar.Value = GameManager.Instance.playerManager.maxPlayerHealthVar.Value;
        playerHealthComponent.Health = GameManager.Instance.playerManager.currentPlayerHealthVar.Value;
    }

    void Update() {

        velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        if (!isRecoiling) thisBody.velocity = velocity * baseSpeed;

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

        // if (Input.GetButtonDown("Reload")) {
        //     // if (GameManager.Instance.playerManager.playerShootingBehaviour.reloadTime >= GameManager.Instance.playerManager.playerShootingBehaviour.currentWeapon.ReloadSpeed &&
        //     //     GameManager.Instance.playerManager.playerShootingBehaviour.currentWeapon.ClipSize < GameManager.Instance.playerManager.playerShootingBehaviour.currentWeapon.MaxClipSize) {
        //     //     StartCoroutine(GameManager.Instance.playerManager.playerShootingBehaviour.ReloadWeaponRoutine());
        //     // }
        //     Recoil(new Vector3(0, 1, 0), 100);
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

        playerShootingBehaviour.ShootWeapon(transform.position + direction, new Vector3(0, 0, startingZRotation));
        
        if (playerShootingBehaviour.currentWeapon.Recoil > 0) {
            StartCoroutine(RecoilRoutine(-direction, playerShootingBehaviour.currentWeapon.Recoil));
        }

        GameManager.Instance.uiManager.ammoSlider.value = playerShootingBehaviour.currentWeapon.ClipSize;
        GameManager.Instance.uiManager.ammoText.text = playerShootingBehaviour.currentWeapon.MaxClipSize > 0 ? "Ammo Left: " + playerShootingBehaviour.currentWeapon.ClipSize.ToString() + "/" + playerShootingBehaviour.currentWeapon.MaxClipSize.ToString() : "Ammo Left: ∞";
    }

    public IEnumerator RecoilRoutine(Vector2 direction, float strength, float delay = 0.7f) {
        isRecoiling = true;
        thisBody.AddForce(direction*strength);
        yield return new WaitForSeconds(delay);
        isRecoiling = false;
    }

    public void GetKnockedBack(Vector2 direction, float strength, float delay = 0.7f) {
        StopCoroutine(RecoilRoutine(direction, strength, delay));
        StartCoroutine(RecoilRoutine(direction, strength, delay));
    }
}
