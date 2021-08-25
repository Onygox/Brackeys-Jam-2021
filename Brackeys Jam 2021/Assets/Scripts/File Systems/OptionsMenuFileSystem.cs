using UnityEngine;
using UnityEngine.UI;
using System.IO;



public class OptionsMenuFileSystem : MonoBehaviour
{
    public Slider sfx , music;
    public void SaveData() {
        string data = JsonUtility.ToJson(new VolumeData(sfx , music));
        System.IO.File.WriteAllText(Application.persistentDataPath + "/OptionsMenu.json", data);
    }

    public void LoadData(){
        string path = Application.persistentDataPath + "/OptionsMenu.json";
        if (File.Exists(path)){
            StreamReader stream = new StreamReader(path); // reads whole file
            VolumeData data = JsonUtility.FromJson<VolumeData>(stream.ReadToEnd()); // create VolumeData object from that json file
            sfx.value = data.sfxVolume;
            music.value = data.musicVolume;
            stream.Close();
        }
        else{
            sfx.value = (sfx.minValue + sfx.maxValue)/2;
            music.value = (music.minValue + music.maxValue)/2;
        }
        
    } 
}