using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    public Slider healthSlider;
    private PlayerScript playerScript;

    [SerializeField] private int _health;
    public int Health {
        get {
            return _health;
        }
        set {
            _health = value;
            healthSlider.value = ExtensionMethods.Remap(_health, 0, MaxHealth, 0, 1);
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
            healthSlider.value = ExtensionMethods.Remap(_health, 0, MaxHealth, 0, 1);
            if (_health <= 0) OnDeath();
        }
    }

    void Start() {
        if (GetComponent<PlayerScript>()) playerScript = GetComponent<PlayerScript>();
    }

    public void TakeDamage(int damageTaken) {
        Health -= damageTaken;
        if (playerScript != null) GameManager.Instance.playerManager.currentPlayerHealthVar.Value = Health;
    }

    void OnDeath() {
        if (playerScript is null) {
            GameObject thisParentObject = transform.parent.gameObject;
            Enemy thisEnemy = thisParentObject.GetComponentInChildren<Enemy>();
            if (GameManager.Instance.enemyManager.currentEnemies.Contains(thisEnemy)) {
                GameManager.Instance.enemyManager.currentEnemies.Remove(thisEnemy);
            }
            Destroy(thisParentObject);
        } else {

        }
    }
}
