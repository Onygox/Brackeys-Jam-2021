using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public UIManager uiManager;
    [HideInInspector] public VolumeManager volumeManager;

    void Start() {

        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        
        uiManager = transform.parent.gameObject.GetComponentInChildren<UIManager>();
        volumeManager = transform.parent.gameObject.GetComponentInChildren<VolumeManager>();
    }
}
