using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ScriptableObject
{
    private float _accuracy;
    public float Accuracy {
        get {
            return _accuracy;
        }
        set {
            _accuracy = value;
        }
    }

    private int _projectileNumber;
    public int ProjectileNumber {
        get {
            return _projectileNumber;
        }
        set {
            _projectileNumber = value;
        }
    }

    private int _reloadSpeed;
    public int ReloadSpeed {
        get {
            return _reloadSpeed;
        }
        set {
            _reloadSpeed = value;
        }
    }

    public GameObject prefab;

    public void FireProjectile() {

    }

    public GameObject InstantiatedWeapon() {
        GameObject newWeapon = Instantiate(prefab);

        return newWeapon;
    }
}
