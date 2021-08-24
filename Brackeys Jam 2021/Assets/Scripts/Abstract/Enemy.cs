using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    void Start() {
        GameManager.Instance.enemyManager.currentEnemies.Add(this);
    }

}
