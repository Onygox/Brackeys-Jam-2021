using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

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

    public void TakeDamage(int damageTaken) {
        Health -= damageTaken;
    }

    void OnDeath() {

    }
}
