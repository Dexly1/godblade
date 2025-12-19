using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public SoundType soundType;
    public enum SoundType
    {
        Enviroment,
        Effect
    }

    public GameObject fromObject;
    public AudioClip clip;
    public bool local;
    public bool single;
    public bool loop;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}

