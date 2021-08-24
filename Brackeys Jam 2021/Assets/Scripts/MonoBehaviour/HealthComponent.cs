using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _health;
    public int Health {
        get {
            return _health;
        }
        set {
            _health = value;
            if (_health <= 0) OnDeath();
        }
    }

    public void TakeDamage(int damageTaken) {
        Health -= damageTaken;
    }

    void OnDeath() {

    }
}
