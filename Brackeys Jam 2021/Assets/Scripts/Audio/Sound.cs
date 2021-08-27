using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip[] clips;

    [Range(0f, 1f)]
    public float volume = 0.8f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    [Range(0f, 1f)]
    public float pitchVariance;

    public bool loop;

    [Range(0f, 1f)]
    public float spatialBlend;
    public float maxDistance;
    public AudioRolloffMode rollOffMode;

    [HideInInspector]
    public AudioSource source;
}
