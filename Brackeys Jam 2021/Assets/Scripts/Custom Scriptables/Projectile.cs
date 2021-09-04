using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Brackeys Jam/Projectile")]
public class Projectile : ScriptableObject
{
    [SerializeField] private int _damage;
    public int Damage {
        get {
            return _damage;
        }
        set {
            _damage = value;
        }
    }
    [SerializeField] private float _radius;
    public float Radius {
        get {
            return _radius;
        }
        set {
            _radius = value;
        }
    }
    [SerializeField] private float _lifetime;
    public float Lifetime {
        get {
            return _lifetime;
        }
        set {
            _lifetime = value;
        }
    }
    [SerializeField] private int _enemiesToPierce;
    public int EnemiesToPierce {
        get {
            return _enemiesToPierce;
        }
        set {
            _enemiesToPierce = value;
        }
    }
    [SerializeField] private float _deceleration;
    public float Deceleration {
        get {
            return _deceleration;
        }
        set {
            _deceleration = value;
        }
    }
    [SerializeField] private float _startingSpeed;
    public float StartingSpeed {
        get {
            return _startingSpeed;
        }
        set {
            _startingSpeed = value;
        }
    }
    [SerializeField] private float _knockback;
    public float Knockback {
        get {
            return _knockback;
        }
        set {
            _knockback = value;
        }
    }
    public bool hasExplosionSound = false;
    public Sound explosionSound;
    [Header("These values are only important if the 'homing' bool is set to true")]
    [SerializeField] private float _homingSpeed;
    public float HomingSpeed {
        get {
            return _homingSpeed;
        }
        set {
            _homingSpeed = value;
        }
    }
    [SerializeField] private float _homingAccuracy;
    public float HomingAccuracy {
        get {
            return _homingAccuracy;
        }
        set {
            _homingAccuracy = value;
        }
    }
    public bool homing, targetOwner, bouncing, friendlyDamage;
    public GameObject prefab;
    // public Sprite projectileImage;
    public Vector3 projectileSize;
    public GameObject explosionEffect;

    public GameObject InstantiatedProjectile() {
        GameObject newProjectile = Instantiate(prefab);
        newProjectile.transform.localScale = projectileSize;

        // SpriteRenderer sr = newProjectile.GetComponentInChildren<SpriteRenderer>();
        // sr.sprite = projectileImage;

        ProjectileScript ps = newProjectile.GetComponent<ProjectileScript>();
        ps.damage = Damage;
        ps.radius = Radius;
        ps.lifetime = Lifetime;
        ps.enemiesToPierce = EnemiesToPierce;
        ps.deceleration = Deceleration;
        ps.startingSpeed = StartingSpeed;
        ps.knockback = Knockback;
        ps.homingSpeed = HomingSpeed;
        ps.homingAccuracy = HomingAccuracy;
        ps.homing = homing;
        ps.bouncing = bouncing;
        ps.friendlyDamage = friendlyDamage;
        ps.targetOwner = targetOwner;
        ps.explosionSound = hasExplosionSound ? explosionSound : null;
        ps.explosionEffect = (explosionEffect is null) ? null : explosionEffect;

        CircleCollider2D bc = newProjectile.AddComponent<CircleCollider2D>();
        bc.isTrigger = !bouncing;

        return newProjectile;
    }
}
