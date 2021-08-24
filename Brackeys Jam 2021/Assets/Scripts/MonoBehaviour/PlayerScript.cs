using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //put movement and visual scripting in here

    float baseSpeed = 2.0f;
    Vector3 velocity;
    private Rigidbody2D thisBody;
    ShootingBehaviour sb;

    void Start() {
        thisBody = GetComponent<Rigidbody2D>();
        sb = GetComponent<ShootingBehaviour>();
    }

    void Update() {
        velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        thisBody.velocity = velocity * baseSpeed;

        if (Input.GetButtonDown("Fire Horizontal")) {
            sb.ShootWeapon(new Vector3(Input.GetAxis("Fire Horizontal"), 0, 0));
        }

        if (Input.GetButtonDown("Fire Vertical")) {
            sb.ShootWeapon(new Vector3(0, Input.GetAxis("Fire Vertical"), 0));
        }

        if (Input.GetButtonDown("Reload")) {
            GameManager.Instance.playerManager.playerShootingBehaviour.ReloadWeapon();
        }
    }
}
