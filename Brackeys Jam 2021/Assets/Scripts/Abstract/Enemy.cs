using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected virtual void Start() {
        GameManager.Instance.enemyManager.currentEnemies.Add(this);
    }

    protected virtual void Start() {
        
    }

}
