using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentManager : MonoBehaviour
{
    public static PersistentManager Instance;

    [HideInInspector] public SoundManager soundManager;
    [HideInInspector] public VolumeManager volumeManager;
    [HideInInspector] public MusicManager musicManager;

    [HideInInspector] public CustomScriptable[] scriptableLibrary;
    [HideInInspector] public TerminalEffect[] terminalEffectLibrary;
    [HideInInspector] public AudioClip[] audioLibrary;
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
        terminalEffectLibrary = Resources.LoadAll<TerminalEffect>("Scriptable Objects/Terminal Effects") as TerminalEffect[];

        if (scriptableLibrary.Length > 0) {
            for (int i = scriptableLibrary.Length - 1; i >= 0; i--) {
                scriptableLibrary[i].Initialize(i);
            }
        }
    }

    void Start() {
        soundManager = GetComponentInChildren<SoundManager>();
        musicManager = GetComponentInChildren<MusicManager>();
        volumeManager = GetComponentInChildren<VolumeManager>();

        if (GameManager.Instance.uiManager.musicSlider) {
            GameManager.Instance.uiManager.musicSlider.value = volumeManager.musicVolumeVar.Value;
        }

        if (GameManager.Instance.uiManager.sfxSlider) {
            GameManager.Instance.uiManager.sfxSlider.value = volumeManager.sfxVolumeVar.Value;
        }
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

    public void LoadSceneByIndex(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }
}
