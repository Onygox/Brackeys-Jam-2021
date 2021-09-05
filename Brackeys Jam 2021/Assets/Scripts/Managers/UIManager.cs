
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
    public GameObject messageCanvas, deathCanvas, winCanvas, choiceCanvas;
    public int pistolIndex = 0;
    public GameObject[] choiceButtons;

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
        PersistentManager.Instance.volumeManager.sfxVolumeVar.Value = newValue;
    }

    public void SetGlobalMusicVolume(float newValue) {
        PersistentManager.Instance.volumeManager.musicVolumeVar.Value = newValue;
    }

    public void DisplayChoices() {
        choiceCanvas.SetActive(true);
        Time.timeScale = 0;
        List<TerminalEffect> tempList = new List<TerminalEffect>();
        foreach(TerminalEffect te in PersistentManager.Instance.terminalEffectLibrary) {
            tempList.Add(te);
        }
        int firstIndex = Mathf.FloorToInt(Random.Range(0, tempList.Count));
        TerminalEffect firstEffect = tempList[firstIndex];
        tempList.RemoveAt(firstIndex);
        int secondIndex = Mathf.FloorToInt(Random.Range(0, tempList.Count));
        TerminalEffect secondEffect = tempList[secondIndex];
        tempList.RemoveAt(secondIndex);

        choiceButtons[0].GetComponent<ChoiceButtonScript>().ChangeEffect(firstEffect);
        choiceButtons[1].GetComponent<ChoiceButtonScript>().ChangeEffect(secondEffect);

        choiceButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = firstEffect.description;
        choiceButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = secondEffect.description;
    }
}
