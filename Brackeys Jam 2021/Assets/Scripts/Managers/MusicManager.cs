using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioClip mainMusic;

    [Range(0f , 1f)]
    public float musicVolume;
    private AudioSource musicPlayer;
    private void Awake() {
        musicPlayer = gameObject.GetComponent<AudioSource>();
        musicPlayer.clip = mainMusic;
        musicPlayer.volume = musicVolume;
        musicPlayer.loop = true;

        musicPlayer.Play();
    }

    public void OnPauseGame() {
        // musicPlayer.volume = musicVolume/2;
        // musicPlayer.pitch = 0.2f;
        musicPlayer.Pause();
    }

    public void OnResumeGame() {
        // musicPlayer.volume = musicVolume;
        // musicPlayer.pitch = 0f;
        musicPlayer.Play();
    }

    public void StopSong() {
        musicPlayer.Stop();
    }


}
