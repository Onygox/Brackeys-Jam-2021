using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.enemyManager.currentSpawners.Add(this);
    }
}
