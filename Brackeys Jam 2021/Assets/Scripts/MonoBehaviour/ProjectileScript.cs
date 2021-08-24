using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    [HideInInspector] public int damage;
    public int environmentLayer, enemyLayer;
    [HideInInspector] public float lifetime, radius;
    [HideInInspector] public bool homing, bouncing;
    public GameObject radiusIndicator;
    public GameObject owner;

    void Start() {
        StartCoroutine("EndLife");
    }

    void OnTriggerEnter2D(Collider2D collider) {

        if (collider.gameObject != owner) {
            if (collider.gameObject.layer == enemyLayer ||
                (!bouncing && collider.gameObject.layer == environmentLayer)) {
                
                DisplayRadius();

                DealDamage(radius, damage);

                Destroy(gameObject);
                
            }
        }

    }

    // void OnCollisionEnter2D(Collision2D collider) {

    //     if (collider.gameObject.layer == enemyLayer ||
    //         (!bouncing && collider.gameObject.layer == environmentLayer)) {
            
    //         DisplayRadius();

    //         DealDamage(radius, damage);

    //         Destroy(gameObject);
            
    //     }

    // }

    void DisplayRadius() {
        GameObject gizmo = Instantiate(radiusIndicator);
        gizmo.transform.position = transform.position;
        gizmo.transform.localScale = Vector3.one*(radius*2);
        Destroy(gizmo, 2);
    }

    void DealDamage(float r, int damageAmount) {
        
        GameObject playerObject = GameManager.Instance.playerManager.playerScript.gameObject;

        if (owner != playerObject) {
            foreach(Enemy enemy in GameManager.Instance.enemyManager.currentEnemies) {
                if (Vector3.Distance(transform.position, enemy.gameObject.transform.position) <= radius) {
                    enemy.gameObject.GetComponent<HealthComponent>().TakeDamage(damageAmount);
                }
            }
        } else if (Vector3.Distance(transform.position, playerObject.transform.position) <= radius) {
            playerObject.GetComponent<HealthComponent>().TakeDamage(damageAmount);
        }

        

    }

    IEnumerator EndLife() {
        yield return new WaitForSeconds(lifetime);

        DisplayRadius();

        DealDamage(radius, damage);

        Destroy(gameObject);
    }
}
