using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentManager : MonoBehaviour
{
    public static PersistentManager Instance;

    [HideInInspector] public SoundManager soundManager;
    [HideInInspector] public VolumeManager volumeManager;

    [HideInInspector] public CustomScriptable[] scriptableLibrary;
    public AudioClip[] audioLibrary;
    [HideInInspector] public Weapon[] weaponLibrary;
    public GameObject[] maps;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        scriptableLibrary = Resources.LoadAll<CustomScriptable>("Scriptable Objects/Custom Scriptables/Production") as CustomScriptable[];
        audioLibrary = Resources.LoadAll<AudioClip>("Sound Effects/Production") as AudioClip[];
        weaponLibrary = Resources.LoadAll<Weapon>("Scriptable Objects/Weapons/Production") as Weapon[];

        if (scriptableLibrary.Length > 0) {
            for (int i = scriptableLibrary.Length - 1; i >= 0; i--) {
                scriptableLibrary[i].Initialize(i);
            }
        }
    }

    void Start() {
        soundManager = GetComponentInChildren<SoundManager>();
        volumeManager = transform.parent.gameObject.GetComponentInChildren<VolumeManager>();
    }

    public CustomScriptable FindVariableBySavePath(string varSavePath) {
        CustomScriptable cVar = null;
        for(int i = scriptableLibrary.Length-1; i >= 0; i--) {
            if (scriptableLibrary[i].SaveDataPath == varSavePath) {
                cVar = scriptableLibrary[i];
                break;
            }
        }
        return cVar;
    }

    public void RestartCurrentScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
