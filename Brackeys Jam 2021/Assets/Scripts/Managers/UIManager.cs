
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;
    List<string> weaponNames = new List<string>();
    public TMP_Dropdown weaponSelector;
    public Slider ammoSlider, fireRateSlider, reloadTimeSlider, playerHealthSlider;
    public TextMeshProUGUI ammoText, currentWeaponText, readyToFireText, terminalsReachedText, terminalsIntactText;
    public GameObject messageCanvas, deathCanvas, winCanvas;
    public int pistolIndex = 0;

    void Start() {
        if (weaponSelector != null) {

            for(int i = 0; i < PersistentManager.Instance.weaponLibrary.Length; i++) {
                weaponNames.Add(PersistentManager.Instance.weaponLibrary[i].name);
                if (PersistentManager.Instance.weaponLibrary[i].name == "Pistol") pistolIndex = i;
            }

            weaponSelector.ClearOptions();
            weaponSelector.AddOptions(weaponNames);
        }

        // Invoke("Initialize", 0.1f);
    }

    // void Initialize() {
    //     if (musicSlider) {
    //         musicSlider.value = PersistentManager.Instance.volumeManager.musicVolumeVar.Value;
    //     }

    //     if (sfxSlider) {
    //         sfxSlider.value = PersistentManager.Instance.volumeManager.sfxVolumeVar.Value;
    //     }
    // }

    public void SendFleetingMessage(Vector3 location, string messageText, float destroyDelay = 3.0f) {
        GameObject temporaryMessage = Instantiate(messageCanvas);
        temporaryMessage.transform.position = location;
        temporaryMessage.GetComponentInChildren<TextMeshProUGUI>().text = messageText;
        Destroy(temporaryMessage, destroyDelay);
    }

    public void SetGlobalSFXVolume(float newValue) {
        PersistentManager.Instance.volumeManager.sfxVolumeVar.Value = ExtensionMethods.Remap(newValue, 0.0f, 1.0f, 0.0f, 0.4f);
    }

    public void SetGlobalMusicVolume(float newValue) {
        PersistentManager.Instance.volumeManager.musicVolumeVar.Value = ExtensionMethods.Remap(newValue, 0.0f, 1.0f, 0.0f, 0.4f);
    }
}
