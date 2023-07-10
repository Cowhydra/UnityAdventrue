using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    private GameObject _soundRoot = null;
    // MP3 Player   -> AudioSource
    // MP3 음원     -> AudioClip
    // 관객(귀)     -> AudioListener

    public void Init()
    {

        if (_soundRoot == null)
        {
            _soundRoot = GameObject.Find("@SoundRoot");
            if (_soundRoot == null)
            {
                _soundRoot = new GameObject { name = "@SoundRoot" };
                Object.DontDestroyOnLoad(_soundRoot);

                string[] soundTypeNames = System.Enum.GetNames(typeof(Define.Sound));
                for (int count = 0; count < soundTypeNames.Length - 1; count++)
                {
                    GameObject go = new GameObject { name = soundTypeNames[count] };
                    _audioSources[count] = go.AddComponent<AudioSource>();
                    go.transform.parent = _soundRoot.transform;
                }

                _audioSources[(int)Define.Sound.Bgm].loop = true;
            }
        }
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }
    public void SelectVolumn(Define.Sound type, float volumLength)
    {
        if (type == Define.Sound.Bgm)
        {
           _audioSources[(int)Define.Sound.Bgm].volume= volumLength;
            
        }
        else
        {
            _audioSources[(int)Define.Sound.Effect].volume = volumLength;
        }
    }
    public float CurrentVolumn(Define.Sound type)
    {
        if (type == Define.Sound.Bgm)
        {
            return _audioSources[(int)Define.Sound.Bgm].volume;

        }
        else
        {
            return _audioSources[(int)Define.Sound.Effect].volume;
        }
    }
    public void Play(string path, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Define.Sound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
        {
            switch (type)
            {
                case Define.Sound.Bgm:
                    path = $"@Resources/Sounds/BGM/{path}";
                    break;
                case Define.Sound.Effect:
                    path = $"@Resources/Sounds/Effect/{path}";
                    break;

            }
          
        }

        AudioClip audioClip = null;
        if (type == Define.Sound.Bgm)
        {
            audioClip = Managers.Resource.Load<AudioClip>($"{path}");
        }
        else
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>($"{path}");
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }

    public void Pause_Bgm()
    {
        _audioSources[(int)Define.Sound.Bgm].Pause();
    }
    public void UnPause_Bgm()
    {
        _audioSources[(int)Define.Sound.Bgm].UnPause();
    }
    public void Stop_Effect()
    {
        _audioSources[(int)Define.Sound.Effect].Stop();
    }
}
