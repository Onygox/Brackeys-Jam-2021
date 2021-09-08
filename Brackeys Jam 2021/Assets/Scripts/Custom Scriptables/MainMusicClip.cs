using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Brackeys Jam/Main Music Clip")]
public class MainMusicClip : ScriptableObject
{
    public AudioClip playThisTrack;
    // public MainMusicClip clipToTransitionTo;
    public MainMusicClip deathSoundToTransitionTo;
    public MainMusicClip transitionToThisTrackAutomatically;
    public MainMusicClip successTrackToTransitionTo;
}
