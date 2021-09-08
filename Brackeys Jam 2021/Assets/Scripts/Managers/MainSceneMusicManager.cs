using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneMusicManager : MonoBehaviour
{
    public MainMusicClip[] mainSceneTracks;
    AudioSource thisSource;
    MainMusicClip clipToPlayNext;
    MainMusicClip currentlyPlayingClip;

    public static MainSceneMusicManager MSMusicManager;

    void Start() {
        if (MSMusicManager is null) MSMusicManager = this;
        thisSource = GetComponent<AudioSource>();
        clipToPlayNext = mainSceneTracks[0];
        StartCoroutine(PlayMusicWithTransitions());
    }

    IEnumerator PlayMusicWithTransitions() {
        while (true) {
            Debug.Log("Now Playing: " + clipToPlayNext.playThisTrack.name);
            thisSource.PlayOneShot(clipToPlayNext.playThisTrack);
            currentlyPlayingClip = clipToPlayNext;
            if (clipToPlayNext.transitionToThisTrackAutomatically) {
                Debug.Log("Up next: " + clipToPlayNext.transitionToThisTrackAutomatically + " in " + currentlyPlayingClip.playThisTrack.length);
                BeginTransition(clipToPlayNext.transitionToThisTrackAutomatically);
            } else {
                Debug.Log("No sound found to automatically transition to.");
                Destroy(this.gameObject, currentlyPlayingClip.playThisTrack.length);
            }
            
            yield return new WaitForSeconds(currentlyPlayingClip.playThisTrack.length);
        }
    }

    public void BeginTransition(MainMusicClip nextClip) {
        clipToPlayNext = nextClip;
    }

    public void OnPlayerDeath() {
        if (currentlyPlayingClip.deathSoundToTransitionTo) {
            Debug.Log("Up next: " + clipToPlayNext.deathSoundToTransitionTo);
            BeginTransition(currentlyPlayingClip.deathSoundToTransitionTo);
        } else {
            Debug.Log("No death sound found to transition to.");
        }
    }

    public void OnLevelCompletion() {
        if (currentlyPlayingClip.successTrackToTransitionTo) {
            Debug.Log("Up next: " + clipToPlayNext.successTrackToTransitionTo);
            BeginTransition(currentlyPlayingClip.successTrackToTransitionTo);
        } else {
            Debug.Log("No success sound found to transition to.");
        }
    }

    public void OnMusicVolumeChanged() {
        if (PersistentManager.Instance.volumeManager) thisSource.volume = PersistentManager.Instance.volumeManager.musicVolumeVar.Value;
    }
}
