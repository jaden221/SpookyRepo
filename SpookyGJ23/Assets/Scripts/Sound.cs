using System;
using UnityEngine;

[Serializable]
public class Sound
{
    [SerializeField] AudioClip clip;
    public AudioClip GetClip
    {
        get
        {
            return clip;
        }
    }
    [SerializeField] float volume = 1;
    public float GetVolume
    {
        get
        {
            return volume;
        }
    }
    [SerializeField] SoundType soundType = SoundType.SFX;
    public SoundType GetSoundType
    {
        get
        {
            return soundType;
        }
    }
    public bool loop = false;

    public enum SoundType
    {
        SFX,
        Music
    }
}