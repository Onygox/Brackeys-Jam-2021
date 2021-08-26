using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAP2D;

public class EnemyScript : Enemy
{
    SAP2DAgent agent;
    SpriteRenderer thisRenderer;
    public float radius, minRadius, maxRadius;
    float timeBetweenShots;
    bool hasBeenSeen, isActive;
    ShootingBehaviour sb;
    public GameObject aim;
    RotateTowardsPlayer rtp;
    protected override void Start() {

        base.Start();

        agent = GetComponent<SAP2DAgent>();
        thisRenderer = GetComponentInChildren<SpriteRenderer>();
        sb = GetComponentInChildren<ShootingBehaviour>();
        rtp = aim.GetComponent<RotateTowardsPlayer>();
        agent.Target = GameManager.Instance.playerManager.playerScript.gameObject.transform;
        agent.CanMove = false;
        isActive = false;
        hasBeenSeen = false;
        timeBetweenShots = 0;
        StartCoroutine("ShootTowardPlayer");
        StartCoroutine("MakeActive");
    }

    void Update() {
        if (!isActive) return;

        if (IsVisible()) hasBeenSeen = true;

        if (hasBeenSeen) {
            if (rtp.PlayerIsVisible()) {
                radius = maxRadius;
                if (IsInRange()) {
                    agent.CanMove = false;
                } else {
                    agent.CanMove = true;
                }
            } else {
                radius = Mathf.Clamp(radius-0.01f, minRadius, maxRadius);
                agent.CanMove = true;
            }
        }
    }

    IEnumerator ShootTowardPlayer() {
        while (true) {
            yield return new WaitForSeconds(0.1f);
            if (IsInRange() && rtp.PlayerIsVisible()) {
                timeBetweenShots+=0.1f;
                if (timeBetweenShots >= sb.currentWeapon.FireRate) {
                    sb.ShootWeapon(transform.position, aim.transform.rotation.eulerAngles);
                    timeBetweenShots = 0;
                }
            }
        }
    }

    IEnumerator MakeActive() {
        yield return new WaitForSeconds(0.2f);
        isActive = true;
    }

    public bool IsVisible() {
        //Check Visibility in main camera viewport

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        bool onScreen = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;

        return (onScreen && thisRenderer.isVisible);
    }

    public bool IsInRange() {
        return (Vector2.Distance(transform.position, GameManager.Instance.playerManager.playerScript.gameObject.transform.position) <= radius);
    }
}
