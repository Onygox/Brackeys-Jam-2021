using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private GameObject soundObject;

    public void PlaySound(int soundIndex, float volume, float variance) {
        GameObject newSound = Instantiate(soundObject);
        AudioSource soundSource = newSound.GetComponent<AudioSource>();
        AudioClip newClip = soundIndex < PersistentManager.Instance.audioLibrary.Length ? PersistentManager.Instance.audioLibrary[soundIndex] : null;
        
        if (newClip is null) {
            Debug.LogWarning("Sound index is out of range");
            return;
        }

        soundSource.volume = volume;
        soundSource.pitch = Random.Range(-variance, variance);

        soundSource.PlayOneShot(newClip);
    }
}
