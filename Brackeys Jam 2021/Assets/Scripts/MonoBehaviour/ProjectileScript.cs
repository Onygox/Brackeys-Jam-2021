using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    [HideInInspector] public int damage;
    public int environmentLayer, enemyLayer, playerLayer, enemiesToPierce, enemiesHit;
    [HideInInspector] public float lifetime, radius, deceleration, startingSpeed, homingSpeed, homingAccuracy, knockback;
    [HideInInspector] public bool homing, bouncing, friendlyDamage;
    public GameObject radiusIndicator;
    public GameObject owner;
    Rigidbody2D thisBody;
    public Transform target;
    private Vector3 vectorToTarget;
    [SerializeField] public GameObject homingAimTarget;

    void Start() {
        StartCoroutine("EndLife");
        enemiesHit = 0;
        thisBody = GetComponentInChildren<Rigidbody2D>();
        if (homing) {
            target = GetClosestEnemy(GameManager.Instance.enemyManager.currentEnemies);
            GameObject aimTarget = Instantiate(homingAimTarget, target.position, Quaternion.identity);
            Destroy(aimTarget, lifetime-0.1f);
        }
    }

    void Update() {
        if (thisBody.velocity.x > 0.001f || thisBody.velocity.x < -0.001f ||
            thisBody.velocity.y > 0.001f || thisBody.velocity.y < -0.001f) {
            thisBody.velocity *= deceleration;
        }

        if (target != null && homing) {
            vectorToTarget = target.position - transform.position;
            float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * homingSpeed);
            thisBody.AddForce(transform.up * homingAccuracy);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {

        GameObject playerObject = GameManager.Instance.playerManager.playerScript.gameObject;

        if (collider.gameObject != owner) {
            //if owned by player, hit the first enemy
            //if not owned by player, no not hit enemies
            if ((collider.gameObject.layer == enemyLayer && owner == playerObject)|| collider.gameObject.layer == playerLayer) {
                
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

        GameObject playerObject = GameManager.Instance.playerManager.playerScript.gameObject;
        Debug.Log("Hit " + collider.gameObject);

        if (collider.gameObject != owner) {
            //if owned by player, hit the first enemy
            //if not owned by player, no not hit enemies
            if ((collider.gameObject.layer == enemyLayer && owner == playerObject) || collider.gameObject.layer == playerLayer) {
                
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
            for(int i = GameManager.Instance.enemyManager.currentEnemies.Count - 1; i >= 0; i--) {

                CircleCollider2D enemyCollider = GameManager.Instance.enemyManager.currentEnemies[i].gameObject.GetComponentInChildren<CircleCollider2D>();
                float realRadius = radius + enemyCollider.radius;

                // Debug.Log("Distance " + Vector2.Distance(transform.position, enemy.gameObject.transform.position));
                // Debug.Log("real radius " + realRadius);

                if (Vector2.Distance(transform.position, GameManager.Instance.enemyManager.currentEnemies[i].gameObject.transform.position) <= realRadius) {
                    GameManager.Instance.enemyManager.currentEnemies[i].gameObject.GetComponentInChildren<HealthComponent>().TakeDamage(damageAmount);

                    //knockback
                    if (knockback > 0) {
                        Vector3 vectorToTarget = GameManager.Instance.enemyManager.currentEnemies[i].gameObject.transform.position - transform.position;
                        // Debug.Log("Hit Vector " + vectorToTarget.normalized);
                        StartCoroutine(GameManager.Instance.enemyManager.currentEnemies[i].gameObject.GetComponent<EnemyScript>().GetKnocked(vectorToTarget, knockback, 0.4f));
                    }
                }
            
            }
            if (friendlyDamage) {
                float radiusToPlayer = radius + GameManager.Instance.playerManager.playerScript.thisCollider.radius;

                if (Vector2.Distance(transform.position, playerObject.transform.position) <= radiusToPlayer) {
                    playerObject.GetComponentInChildren<HealthComponent>().TakeDamage(damageAmount);

                    //knockback
                    if (knockback > 0) {
                        Vector3 vectorToTarget = playerObject.gameObject.transform.position - transform.position;
                        // Debug.Log("Hit Vector " + vectorToTarget.normalized);
                        StartCoroutine(GameManager.Instance.playerManager.playerScript.RecoilRoutine(vectorToTarget, knockback));
                    }
                }
            }
        } else {
            float realRadius = radius + GameManager.Instance.playerManager.playerScript.thisCollider.radius;

            // Debug.Log("Distance " + Vector2.Distance(transform.position, playerObject.transform.position));
            // Debug.Log("real radius " + realRadius);

            if (Vector2.Distance(transform.position, playerObject.transform.position) <= realRadius) {
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

    //from edwardrowe at https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
    Transform GetClosestEnemy (List<Enemy> enemies) {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(Enemy potentialTarget in enemies) {
            Transform enemyTransform = potentialTarget.gameObject.transform;
            Vector3 directionToTarget = enemyTransform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = enemyTransform;
            }
        }
     
        return bestTarget;
    }
}
