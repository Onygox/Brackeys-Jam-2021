using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
    GameObject target;
    public float speed = 10.0f;
    Vector2 vectorToTarget;
    public LayerMask layersToSee;
    void Start() {
        target = GameManager.Instance.playerManager.playerScript.gameObject;
    }

    void Update() {
        vectorToTarget = target.transform.position - transform.position;
        float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
    }

    public bool PlayerIsVisible() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, vectorToTarget, 100, layersToSee);

        return (hit.collider.gameObject == target);
    }
}
