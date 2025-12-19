using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager single;
    public List<AudioSource> _audioSources;
    public bool _mutedByPlayer = false;
    public Sound[] sounds;
    public string _currentBattleMusic;

    private void Awake()
    {
        if (single == null)
        {
            single = this;
            SetSounds();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    public void SetSounds()
    {
        foreach (Sound s in sounds)
        {
            AudioSource newSource = null;

            if (s.fromObject.GetComponent<AudioSource>() == null || s.single)
            {
                AudioSource source = s.fromObject.AddComponent<AudioSource>();
                source.playOnAwake = false;

                if (!s.local)
                {
                    source.maxDistance = 10f;
                    source.spatialBlend = 1f;
                }

                newSource = source;
            }

            if (newSource != null)
                s.source = newSource;
            else
                s.source = s.fromObject.GetComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;

            s.source.loop = s.loop;

            //switch (s.soundType)
            //{
            //    case Sound.SoundType.Enviroment:
            //        s.source.volume = SettingsSys.single._enviromentVolume / 1f * SettingsSys.single._masterVolume * s.volume;
            //        break;
            //    case Sound.SoundType.Speech:
            //        s.source.volume = SettingsSys.single._speechesVolume / 1f * SettingsSys.single._masterVolume * s.volume;
            //        break;
            //    case Sound.SoundType.Effect:
            //        s.source.volume = SettingsSys.single._effectsVolume / 1f * SettingsSys.single._masterVolume * s.volume;
            //        break;
            //}
            s.source.pitch = s.pitch;

            _audioSources.Add(s.source);
        }
    }

    public void MuteVolumeByPlayer()
    {
        _mutedByPlayer = !_mutedByPlayer;

        for (int i = 0; i < _audioSources.Count; i++)
            _audioSources[i].mute = _mutedByPlayer;
    }

    public void MuteVolume(bool value)
    {
        if (!_mutedByPlayer)
            for (int i = 0; i < _audioSources.Count; i++)
                _audioSources[i].mute = value;
    }

    public void Play(string name, bool rise = false)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (rise)
        {
            s.source.volume = 0f;
            StartCoroutine(RiseSound(s, 1f));
        }
        else
        {
            s.source.volume = s.volume;
        }

        s.source.Play();
    }

    public void Stop(string name, bool longFade = false)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (longFade)
            StartCoroutine(FadeSound(s, 1f));
        else
            StartCoroutine(FadeSound(s, 1f));
    }

    public IEnumerator FadeSound(Sound s, float length)
    {
        while (true)
        {
            s.source.volume -= Time.deltaTime / length;
            if (s.source.volume <= 0f)
            {
                break;
            }

            yield return null;
        }

        s.source.volume = 0;
    }

    public IEnumerator RiseSound(Sound s, float length)
    {
        while (true)
        {
            s.source.volume += Time.deltaTime / length;
            if (s.source.volume >= s.volume)
            {
                break;
            }

            yield return null;
        }

        s.source.volume = s.volume;
    }
}
