using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{

    public ScriptableFloat musicVolumeVar, sfxVolumeVar;

    void Start() {
        musicVolumeVar = PersistentManager.Instance.FindVariableBySavePath("musicvolumedata") as ScriptableFloat;
        sfxVolumeVar = PersistentManager.Instance.FindVariableBySavePath("sfxvolumedata") as ScriptableFloat;
    }

    public void AdjustMusicVolume(float newVol) {
        musicVolumeVar.Value = newVol;
    }

    public void AdjustEffectsVolume(float newVol) {
        sfxVolumeVar.Value = newVol;
    }
}
