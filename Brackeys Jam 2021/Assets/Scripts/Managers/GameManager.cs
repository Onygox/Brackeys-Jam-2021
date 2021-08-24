using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public UIManager uiManager;
    [HideInInspector] public VolumeManager volumeManager;
    [HideInInspector] public PlayerManager playerManager;
    [HideInInspector] public MapManager mapManager;
    [HideInInspector] public EnemyManager enemyManager;

    public CinemachineVirtualCamera vcam;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        uiManager = transform.parent.gameObject.GetComponentInChildren<UIManager>();
        volumeManager = transform.parent.gameObject.GetComponentInChildren<VolumeManager>();
        playerManager = transform.parent.gameObject.GetComponentInChildren<PlayerManager>();
        mapManager = transform.parent.gameObject.GetComponentInChildren<MapManager>();
        enemyManager = transform.parent.gameObject.GetComponentInChildren<EnemyManager>();

        StartGame(0);
    }

    void StartGame(int level) {
        mapManager.CreateMap(PersistentManager.Instance.maps[level]);
    }
}
