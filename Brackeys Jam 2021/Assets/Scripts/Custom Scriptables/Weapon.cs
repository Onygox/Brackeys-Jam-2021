using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Events;

[CreateAssetMenu(menuName="Brackeys Jam/Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField] private float _accuracy;
    public float Accuracy {
        get {
            return _accuracy;
        }
        set {
            _accuracy = value;
        }
    }

    [SerializeField] private int _projectileNumber;
    public int ProjectileNumber {
        get {
            return _projectileNumber;
        }
        set {
            _projectileNumber = value;
        }
    }

    [SerializeField] private float _reloadSpeed;
    public float ReloadSpeed {
        get {
            return _reloadSpeed;
        }
        set {
            _reloadSpeed = value;
        }
    }
    [SerializeField] private float _fireRate;
    public float FireRate {
        get {
            return _fireRate;
        }
        set {
            _fireRate = value;
        }
    }
    [SerializeField] private int _clipSize;
    public int ClipSize {
        get {
            return _clipSize;
        }
        set {
            _clipSize = value;
        }
    }
    [SerializeField] private int _maxClipSize;
    public int MaxClipSize {
        get {
            return _maxClipSize;
        }
        set {
            _maxClipSize = value;
        }
    }
    [SerializeField] private float _spreadAngle;
    public float SpreadAngle {
        get {
            return _spreadAngle;
        }
        set {
            _spreadAngle = value;
        }
    }
    [SerializeField] private float _power;
    public float Power {
        get {
            return _power;
        }
        set {
            _power = value;
        }
    }

    public Sprite weaponImage;
    public bool automatic;
    public GameEvent onShootEvent;
    public Projectile projectileType;
    public GameObject prefab;

    public void Shoot(Vector3 spawnLocation, Vector3 direction, GameObject owner) {

        if (ClipSize <= 0) {
            return;
        }

        // Debug.Log("Weapon shot at " + spawnLocation + " in " + direction + " direction");

        int startingZRotation = 0;

        switch (direction.x) {

            case 1:
                startingZRotation = 270;
                break;
            case -1:
                startingZRotation = 90;
                break;
            case 0:
                startingZRotation = direction.y > 0 ? 0 : 180;
                break;
            default:
                break;

        }

        for(int i = 1; i <= ProjectileNumber; i++) {

            float newZRotation;

            float leastAngle = startingZRotation - (SpreadAngle/2);
            float mostAngle = startingZRotation + (SpreadAngle/2);

            newZRotation = ExtensionMethods.Remap(i, 1, ProjectileNumber, leastAngle, mostAngle);

            newZRotation += Random.Range(((Accuracy/2) - 50), (50 - (Accuracy/2)));

            Vector3 projectileRotation = ProjectileNumber == 1 ? new Vector3(0, 0, startingZRotation) : new Vector3(0, 0, newZRotation);

            GameObject newProjectile = projectileType.InstantiatedProjectile();
            newProjectile.transform.position = spawnLocation;
            newProjectile.transform.rotation = Quaternion.Euler(projectileRotation);

            newProjectile.GetComponent<Rigidbody2D>().AddForce(Power * projectileType.StartingSpeed * newProjectile.transform.up);

            ProjectileScript ps = newProjectile.GetComponent<ProjectileScript>();
            ps.owner = owner;

        }

        onShootEvent?.Raise();
    }

    public void Reload() {
        ClipSize = MaxClipSize;
    }

    public GameObject InstantiatedWeapon() {
        GameObject newWeapon = Instantiate(prefab);

        return newWeapon;
    }
}
