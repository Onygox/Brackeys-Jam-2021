using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private GameObject soundObject;

    public void PlaySound(int soundIndex, float volume = 0.4f, float variance = 0.2f) {
        GameObject newSound = Instantiate(soundObject);
        AudioSource soundSource = newSound.GetComponent<AudioSource>();
        AudioClip newClip = soundIndex < PersistentManager.Instance.audioLibrary.Length ? PersistentManager.Instance.audioLibrary[soundIndex] : null;
        
        if (newClip is null) {
            Debug.LogWarning("Sound index is out of range");
            return;
        }

        // soundSource.clip = newClip;
        soundSource.volume = volume * PersistentManager.Instance.volumeManager.sfxVolumeVar.Value;
        soundSource.pitch += Random.Range(-variance, variance);

        soundSource.PlayOneShot(newClip);

        Destroy(newSound, newClip.length);
    }

    public void PlayRandomPlayerFootstepSound() {
        AudioClip randomFootstep = PersistentManager.Instance.playerFootstepLibrary[Mathf.FloorToInt(Random.Range(0, PersistentManager.Instance.playerFootstepLibrary.Length))];
        
        GameObject newSound = Instantiate(soundObject);
        AudioSource soundSource = newSound.GetComponent<AudioSource>();
        AudioClip newClip = randomFootstep;

        soundSource.volume = 0.4f * PersistentManager.Instance.volumeManager.sfxVolumeVar.Value;
        soundSource.pitch += Random.Range(-0.2f, 0.2f);

        soundSource.PlayOneShot(newClip);

        Destroy(newSound, newClip.length);
    }

    public void PlayRandomEnemyFootstepSound() {
        AudioClip randomFootstep = PersistentManager.Instance.enemyFootstepLibrary[Mathf.FloorToInt(Random.Range(0, PersistentManager.Instance.enemyFootstepLibrary.Length))];
        
        GameObject newSound = Instantiate(soundObject);
        AudioSource soundSource = newSound.GetComponent<AudioSource>();
        AudioClip newClip = randomFootstep;

        soundSource.volume = 0.4f * PersistentManager.Instance.volumeManager.sfxVolumeVar.Value;
        soundSource.pitch += Random.Range(-0.2f, 0.2f);

        soundSource.PlayOneShot(newClip);

        Destroy(newSound, newClip.length);
    }
}
