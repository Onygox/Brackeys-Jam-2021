using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    public static PersistentManager Instance;

    [HideInInspector] public SoundManager soundManager;

    void Start() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        soundManager = GetComponentInChildren<SoundManager>();
    }
}
