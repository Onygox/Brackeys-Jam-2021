using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : ScriptableObject
{
    private int _damage;
    public int Damage {
        get {
            return _damage;
        }
        set {
            _damage = value;
        }
    }
    public GameObject prefab;

    public GameObject InstantiatedProjectile() {
        GameObject newProjectile = Instantiate(prefab);

        return newProjectile;
    }
}
