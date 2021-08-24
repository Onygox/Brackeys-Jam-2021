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
    public Slider ammoSlider, fireRateSlider, reloadTimeSlider;
    public TextMeshProUGUI ammoText;

    void Start()
    {
        foreach(Weapon w in PersistentManager.Instance.weaponLibrary) {
            weaponNames.Add(w.name);
        }

        weaponSelector.ClearOptions();
        weaponSelector.AddOptions(weaponNames);
    }
}
