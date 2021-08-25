using UnityEngine.UI;

[System.Serializable]
public class VolumeData{
    public float sfxVolume;
    public float musicVolume;

    public VolumeData(Slider sfx , Slider music){ // constructor for class
        sfxVolume = sfx.value;
        musicVolume = music.value;
    }
}