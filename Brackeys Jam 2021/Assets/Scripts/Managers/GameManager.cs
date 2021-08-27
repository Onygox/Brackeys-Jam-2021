using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector] public UIManager uiManager;
    [HideInInspector] public PlayerManager playerManager;
    [HideInInspector] public MapManager mapManager;
    [HideInInspector] public EnemyManager enemyManager;
    private int levelInt;
    public int startingLevel = 0;

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
        playerManager = transform.parent.gameObject.GetComponentInChildren<PlayerManager>();
        mapManager = transform.parent.gameObject.GetComponentInChildren<MapManager>();
        enemyManager = transform.parent.gameObject.GetComponentInChildren<EnemyManager>();

        StartGame(startingLevel);
        
    }

    void StartGame(int level) {

        if (PersistentManager.Instance.maps.Length < level - 1) {
            Debug.LogWarning("level number out of range");
            return;
        }
        // mapManager.CreateMap(PersistentManager.Instance.maps[level]);
        levelInt = level;
        StartCoroutine("StartGameRoutine");
    }

    IEnumerator StartGameRoutine() {
        yield return new WaitForSeconds(0.1f);
        mapManager.CreateMap(PersistentManager.Instance.maps[levelInt]);
    }

    public void RestartCurrentScene() {
        PersistentManager.Instance.RestartCurrentScene();
    }
}
