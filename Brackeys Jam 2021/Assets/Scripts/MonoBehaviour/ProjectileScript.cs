using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    [HideInInspector] public int damage;
    public int environmentLayer, enemyLayer, enemiesToPierce, enemiesHit;
    [HideInInspector] public float lifetime, radius, deceleration, startingSpeed;
    [HideInInspector] public bool homing, bouncing;
    public GameObject radiusIndicator;
    public GameObject owner;
    Rigidbody2D thisBody;
    GameObject target;

    void Start() {
        StartCoroutine("EndLife");
        enemiesHit = 0;
        thisBody = GetComponentInChildren<Rigidbody2D>();
        if (homing) {
            
        }
    }

    void Update() {
        if (thisBody.velocity.x > 0.001f || thisBody.velocity.x < -0.001f ||
            thisBody.velocity.y > 0.001f || thisBody.velocity.y < -0.001f) {
            thisBody.velocity *= deceleration;
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {

        if (collider.gameObject != owner) {
            if (collider.gameObject.layer == enemyLayer) {
                
                DisplayRadius();

                DealDamage(radius, damage);

                enemiesHit++;

                if (enemiesHit >= enemiesToPierce) {
                    Destroy(gameObject);
                }
                
            } else if (!bouncing && collider.gameObject.layer == environmentLayer) {

                DisplayRadius();

                DealDamage(radius, damage);

                Destroy(gameObject);

            }
        }

    }

    void OnCollisionEnter2D(Collision2D collider) {

        if (collider.gameObject != owner) {
            if (collider.gameObject.layer == enemyLayer) {
                
                DisplayRadius();

                DealDamage(radius, damage);

                enemiesHit++;

                if (enemiesHit >= enemiesToPierce) {
                    Destroy(gameObject);
                }
                
            } else if (!bouncing && collider.gameObject.layer == environmentLayer) {

                DisplayRadius();

                DealDamage(radius, damage);

                Destroy(gameObject);

            }
        }

    }

    void DisplayRadius() {
        GameObject gizmo = Instantiate(radiusIndicator);
        gizmo.transform.position = transform.position;
        gizmo.transform.localScale = Vector3.one*(radius*2);
        Destroy(gizmo, 2);
    }

    void DealDamage(float r, int damageAmount) {
        
        GameObject playerObject = GameManager.Instance.playerManager.playerScript.gameObject;

        if (owner == playerObject) {
            foreach(Enemy enemy in GameManager.Instance.enemyManager.currentEnemies) {

                CircleCollider2D enemyCollider = enemy.gameObject.GetComponentInChildren<CircleCollider2D>();
                float realRadius = radius + enemyCollider.radius;

                // Debug.Log("Distance " + Vector3.Distance(transform.position, enemy.gameObject.transform.position));
                // Debug.Log("real radius " + realRadius);

                if (Vector3.Distance(transform.position, enemy.gameObject.transform.position) <= realRadius) {
                    enemy.gameObject.GetComponentInChildren<HealthComponent>().TakeDamage(damageAmount);
                }
            }
        } else {
            float realRadius = radius + GameManager.Instance.playerManager.playerScript.thisCollider.radius;

            if (Vector3.Distance(transform.position, playerObject.transform.position) <= realRadius) {
                // Debug.Log("Distance " + Vector3.Distance(transform.position, playerObject.transform.position));
                // Debug.Log("real radius " + realRadius);

                playerObject.GetComponentInChildren<HealthComponent>().TakeDamage(damageAmount);
            }
        }

        

    }

    IEnumerator EndLife() {
        yield return new WaitForSeconds(lifetime);

        DisplayRadius();

        DealDamage(radius, damage);

        Destroy(gameObject);
    }
}
