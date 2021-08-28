using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAP2D;

public class EnemyScript : Enemy
{
    public SAP2DAgent agent;
    SpriteRenderer thisRenderer;
    public float radius, minRadius, maxRadius;
    float timeBetweenShots;
    bool hasBeenSeen, isActive;
    ShootingBehaviour sb;
    HealthComponent hc;
    public GameObject aim;
    RotateTowardsPlayer rtp;
    public Animator headAnim, bodyAnim;

    protected override void Start() {

        base.Start();

        thisRenderer = GetComponentInChildren<SpriteRenderer>();
        sb = GetComponentInChildren<ShootingBehaviour>();
        rtp = aim.GetComponent<RotateTowardsPlayer>();
        agent = GetComponent<SAP2DAgent>();
        hc = GetComponent<HealthComponent>();

        agent.Target = GameManager.Instance.playerManager.playerScript.gameObject.transform;
        agent.CanMove = false;
        isActive = false;
        hasBeenSeen = false;
        timeBetweenShots = 0;
        hc.indestructible = true;

        StartCoroutine("ShootTowardPlayer");
        StartCoroutine("MakeActive");
        StartCoroutine("PlayFootstepNoises");
    }

    void Update() {
        if (GameManager.Instance.GameIsOver()) return;

        if (!isActive) {
            hc.indestructible = true;
            return;
        }

        if (IsVisible()) {
            hasBeenSeen = true;
            hc.indestructible = false;
        }

        if (hasBeenSeen && !isBeingKnocked) {
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
        } else {
            agent.CanMove = false;
        }

        bodyAnim.SetBool("Active", agent.CanMove);
        headAnim.SetBool("Active", rtp.PlayerIsVisible());
    }

    IEnumerator ShootTowardPlayer() {
        while (true) {
            yield return new WaitForSeconds(0.1f);
            if (IsInRange() && rtp.PlayerIsVisible() && !GameManager.Instance.GameIsOver()) {
                timeBetweenShots+=0.1f;
                if (timeBetweenShots >= sb.currentWeapon.FireRate) {
                    Vector2 normalizedDirection = rtp.vectorToTarget.normalized;
                    sb.ShootWeapon(transform.position + new Vector3(normalizedDirection.x, normalizedDirection.y, 0), aim.transform.rotation.eulerAngles);
                    timeBetweenShots = 0;
                }
            }
        }
    }

    IEnumerator MakeActive() {
        yield return new WaitForSeconds(0.3f);
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

    public IEnumerator PlayFootstepNoises() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
            if (IsVisible() && !IsInRange()) {
                PersistentManager.Instance.soundManager.PlayRandomEnemyFootstepSound();
            }
        }
    }
}
