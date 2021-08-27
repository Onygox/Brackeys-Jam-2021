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

    private int numberOfActiveTerminals = 0;

    public int NumberOfActiveTerminals {
        get {
            return numberOfActiveTerminals;
        }
        set {
             numberOfActiveTerminals = value;
             if (numberOfActiveTerminals >= mapManager.terminalsInLevel.Count) EndGame(true);
        }
    }

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
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.1f);
        mapManager.CreateMap(PersistentManager.Instance.maps[levelInt]);
    }

    public void RestartCurrentScene() {
        Time.timeScale = 1;
        PersistentManager.Instance.RestartCurrentScene();
    }

    public void EndGame(bool win) {

        if (win) {
            //if the current level is less than the total number of levels, progress to the next one
            //otherwise, go to the end game scene
            uiManager.winCanvas.SetActive(true);

            if (levelInt < PersistentManager.Instance.maps.Length - 1) {
                StartCoroutine(LoadSceneWithDelay(levelInt+1, 2.0f));
            } else {
                StartCoroutine(LoadEndSceneWithDelay());
            }

        } else {
            uiManager.deathCanvas.SetActive(true);
        }

    }

    IEnumerator LoadSceneWithDelay(int levelIndex, float delayFloat) {
        yield return new WaitForSeconds(delayFloat);
        StartGame(levelIndex);
    }

    IEnumerator LoadEndSceneWithDelay() {
        yield return new WaitForSeconds(2.0f);
        PersistentManager.Instance.LoadSceneByIndex(2);
    }

    public bool GameIsOver() {
        return (uiManager.deathCanvas.activeSelf || uiManager.winCanvas.activeSelf);
    }
}
