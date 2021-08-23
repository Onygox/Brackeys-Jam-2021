using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //put movement and visual scripting in here

    float baseSpeed = 2.0f;
    Vector3 velocity;
    [SerializeField] private Rigidbody2D thisBody;

    void Update() {
        velocity = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        thisBody.velocity = velocity * baseSpeed;
        // thisBody.AddForce(velocity * baseSpeed, ForceMode2D.Force);
    }
}
