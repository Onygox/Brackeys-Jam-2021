using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {
    //from Jessy on https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
    public static float Remap(this float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
// [System.Serializable]
// public class MainMusicClip {
//     public AudioClip playThisTrack;
//     // public bool loop;
//     [SerializeField]
//     public TransitionalClip clipToTransitionTo;
// }
// [System.Serializable]
// public class TransitionalClip {
//     public AudioClip playThisTrack;
//     [SerializeField]
//     public MainMusicClip clipToTransitionTo;
// }
