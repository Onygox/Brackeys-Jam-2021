using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
    GameObject target;
    public float speed = 10.0f;
    public Vector2 vectorToTarget;
    public LayerMask layersToSee;
    void Start() {
        target = GameManager.Instance.playerManager.playerScript.gameObject;
    }

    void Update() {
        if (target is null) return;
        vectorToTarget = target.transform.position - transform.position;
        float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
    }

    public bool PlayerIsVisible() {

        if (target is null) return false;

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + new Vector3(0.12f, -0.12f, 0.0f), vectorToTarget, 100, layersToSee);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + new Vector3(-0.12f, 0.12f, 0.0f), vectorToTarget, 100, layersToSee);
        RaycastHit2D hit3 = Physics2D.Raycast(transform.position + new Vector3(-0.12f, -0.12f, 0.0f), vectorToTarget, 100, layersToSee);
        RaycastHit2D hit4 = Physics2D.Raycast(transform.position + new Vector3(0.12f, 0.12f, 0.0f), vectorToTarget, 100, layersToSee);

        return (hit1.collider.gameObject == target && hit2.collider.gameObject == target && hit3.collider.gameObject == target && hit4.collider.gameObject == target);
    }
}
