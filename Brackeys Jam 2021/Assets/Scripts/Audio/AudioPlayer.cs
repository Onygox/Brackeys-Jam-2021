using System;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            SetUpSound(s, gameObject.AddComponent<AudioSource>());
        }
    }

    public static void SetUpSound(Sound s, AudioSource source)
    {
        s.source = source;
        s.source.clip = s.clips[0];
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
        s.source.spatialBlend = s.spatialBlend;
        s.source.maxDistance = s.maxDistance;
        s.source.rolloffMode = s.rollOffMode;
    }

    public void Play(string name)
    {
        // find sound
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " could not be found!");
            return;
        }

        PlaySFX(s);
    }

    public static void PlaySFX(Sound s)
    {
        // randomise clip
        if (s.clips.Length > 1)
            s.source.clip = s.clips[UnityEngine.Random.Range(0, s.clips.Length)];

        // randomise pitch
        if (s.pitchVariance != 1)
        {
            float lowerBound = s.pitch - (s.pitchVariance / 2);
            s.source.pitch = UnityEngine.Random.Range(lowerBound, lowerBound + s.pitchVariance);
        }

        s.source.Play();
    }
}