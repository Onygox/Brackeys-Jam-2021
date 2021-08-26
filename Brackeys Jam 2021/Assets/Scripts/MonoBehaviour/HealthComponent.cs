using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    public Slider healthSlider;
    private PlayerScript playerScript;
    public bool indestructible;

    [SerializeField] private int _health;
    public int Health {
        get {
            return _health;
        }
        set {
            _health = value;
            if (healthSlider != null) healthSlider.value = ExtensionMethods.Remap(_health, 0, MaxHealth, 0, 1);
            if (_health <= 0) OnDeath();
        }
    }
    [SerializeField] private int _maxHealth;
    public int MaxHealth {
        get {
            return _maxHealth;
        }
        set {
            _maxHealth = value;
            if (healthSlider != null) healthSlider.value = ExtensionMethods.Remap(_health, 0, MaxHealth, 0, 1);
            if (_health <= 0) OnDeath();
        }
    }

    void Start() {

        indestructible = false;
        
        if (GetComponent<PlayerScript>()) playerScript = GetComponent<PlayerScript>();

        if (GetComponent<Obstacle>()) indestructible = !GetComponent<Obstacle>().destructable;
    }

    public void TakeDamage(int damageTaken) {

        if (indestructible) return;

        Health -= damageTaken;
        if (playerScript != null) GameManager.Instance.playerManager.currentPlayerHealthVar.Value = Health;
    }

    void OnDeath() {
        if (playerScript is null) {
            Enemy thisEnemy = GetComponent<Enemy>();
            if (thisEnemy != null && GameManager.Instance.enemyManager.currentEnemies.Contains(thisEnemy)) {
                GameManager.Instance.enemyManager.currentEnemies.Remove(thisEnemy);
            }
            Obstacle thisObstacle = GetComponent<Obstacle>();
            if (thisObstacle != null && GameManager.Instance.mapManager.destructableObjects.Contains(thisObstacle)) {
                GameManager.Instance.mapManager.destructableObjects.Remove(thisObstacle);
            }
            Destroy(gameObject);
        } else {

        }
    }
}
