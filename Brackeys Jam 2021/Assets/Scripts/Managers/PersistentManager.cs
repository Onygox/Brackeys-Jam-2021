using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    public static PersistentManager Instance;

    [HideInInspector] public SoundManager soundManager;

    [HideInInspector] public CustomScriptable[] scriptableLibrary;
    [HideInInspector] public AudioClip[] audioLibrary;
    public GameObject[] maps;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        scriptableLibrary = Resources.LoadAll<CustomScriptable>("Custom Scriptables/Production") as CustomScriptable[];
        audioLibrary = Resources.LoadAll<AudioClip>("Sound Effects/Production") as AudioClip[];

        if (scriptableLibrary.Length > 0) {
            for (int i = scriptableLibrary.Length - 1; i >= 0; i--) {
                scriptableLibrary[i].Initialize(i);
            }
        }
    }

    void Start() {
        soundManager = GetComponentInChildren<SoundManager>();
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
}
