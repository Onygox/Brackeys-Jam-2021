using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    public Rigidbody2D thisBody;
    protected virtual void Start() {
        GameManager.Instance.enemyManager.currentEnemies.Add(this);

        if (GetComponentInChildren<Rigidbody2D>()) {
            thisBody = GetComponentInChildren<Rigidbody2D>();
        }
    }

}
