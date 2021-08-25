using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
    GameObject target;
    public float speed = 10.0f;
    void Start()
    {
        target = GameManager.Instance.playerManager.playerScript.gameObject;
    }

    void Update()
    {
        Vector3 vectorToTarget = target.transform.position - transform.position;
        // float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
    }
}
