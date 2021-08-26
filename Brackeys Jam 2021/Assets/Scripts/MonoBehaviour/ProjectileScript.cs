using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    public int damage, enemiesToPierce, enemiesHit;
    public int environmentLayer, enemyLayer, playerLayer;
    [HideInInspector] public float lifetime, radius, deceleration, startingSpeed, homingSpeed, homingAccuracy, knockback;
    [HideInInspector] public bool homing, bouncing, friendlyDamage, targetOwner;
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

            target = targetOwner ? owner.transform : GetClosestEnemy(GameManager.Instance.enemyManager.currentEnemies);
            GameObject aimTarget = Instantiate(homingAimTarget, target.position, Quaternion.identity);
            aimTarget.transform.SetParent(target);
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
            //if not owned by player, do not hit enemies
            if ((collider.gameObject.layer == enemyLayer && owner == playerObject)||
                (collider.gameObject.layer == playerLayer)) {
                
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
        } else if (friendlyDamage) {
            DisplayRadius();

            DealDamage(radius, damage);

            enemiesHit++;

            if (enemiesHit >= enemiesToPierce) {
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

                GameObject iteratedEnemy = GameManager.Instance.enemyManager.currentEnemies[i].gameObject;
                CircleCollider2D enemyCollider = iteratedEnemy.GetComponentInChildren<CircleCollider2D>();
                float realRadius = radius + enemyCollider.radius;

                if (Vector2.Distance(transform.position, iteratedEnemy.transform.position) <= realRadius) {
                    iteratedEnemy.GetComponentInChildren<HealthComponent>().TakeDamage(damageAmount);

                    //knockback
                    if (knockback > 0) {
                        Vector2 vectorToTarget = (iteratedEnemy.transform.position - transform.position).normalized;
                        
                        float xVector, yVector;
                        if (vectorToTarget.x > 0) {
                            xVector = 1 - vectorToTarget.x;
                        } else {
                            xVector = - 1 - vectorToTarget.x;
                        }
                        if (vectorToTarget.y > 0) {
                            yVector = 1 - vectorToTarget.y;
                        } else {
                            yVector = - 1 - vectorToTarget.y;
                        }
                        Vector2 realVector = new Vector2(xVector, yVector);
                        GameManager.Instance.enemyManager.currentEnemies[i].GetKnockedBack(realVector.normalized, knockback);
                    }
                }
            
            }

            DealDamageToEnvironment(r, damageAmount);

            //deal damage to player if friendly fire is on
            if (friendlyDamage) {
                float radiusToPlayer = radius + GameManager.Instance.playerManager.playerScript.thisCollider.radius;

                if (Vector2.Distance(transform.position, playerObject.transform.position) <= radiusToPlayer) {
                    playerObject.GetComponentInChildren<HealthComponent>().TakeDamage(damageAmount);

                    //knockback
                    if (knockback > 0) {
                        Vector2 vectorToTarget = (playerObject.gameObject.transform.position - transform.position).normalized;
                        
                        float xVector, yVector;
                        if (vectorToTarget.x > 0) {
                            xVector = 1 - vectorToTarget.x;
                        } else {
                            xVector = - 1 - vectorToTarget.x;
                        }
                        if (vectorToTarget.y > 0) {
                            yVector = 1 - vectorToTarget.y;
                        } else {
                            yVector = - 1 - vectorToTarget.y;
                        }
                        Vector2 realVector = new Vector2(xVector, yVector);
                        GameManager.Instance.playerManager.playerScript.GetKnockedBack(realVector.normalized, knockback);
                    }
                }
            }
        } else {
            float realRadius = radius + GameManager.Instance.playerManager.playerScript.thisCollider.radius;

            if (Vector2.Distance(transform.position, playerObject.transform.position) <= realRadius) {
                playerObject.GetComponentInChildren<HealthComponent>().TakeDamage(damageAmount);
            }

            DealDamageToEnvironment(r, damageAmount);
        }
    }

    void DealDamageToEnvironment(float r, int damageAmount) {
        for (int i = GameManager.Instance.mapManager.destructableObjects.Count - 1; i >= 0; i--) {

            GameObject iteratedObstacle = GameManager.Instance.mapManager.destructableObjects[i].gameObject;
            BoxCollider2D obstacleCollider = iteratedObstacle.GetComponentInChildren<BoxCollider2D>();
            float realRadius = radius + obstacleCollider.size.x;

            if (Vector2.Distance(transform.position, iteratedObstacle.transform.position) <= realRadius) {
                iteratedObstacle.GetComponentInChildren<HealthComponent>().TakeDamage(damageAmount);
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
            if(dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = enemyTransform;
            }
        }
     
        return bestTarget;
    }
}
