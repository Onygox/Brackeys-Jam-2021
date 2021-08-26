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
    public TextMeshProUGUI ammoText, currentWeaponText, readyToFireText;
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
    }
}
