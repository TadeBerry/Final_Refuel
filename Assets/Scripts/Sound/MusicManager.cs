using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{


    private const int MUSIC_VOLUME_MAX = 10;


    public static MusicManager Instance { get; private set; }


    private static float musicTime;
    private static int musicVolume = 2;


    public event EventHandler OnMusicVolumeChanged;


    private AudioSource musicAudioSource;


    private void Awake()
{
    musicAudioSource = GetComponent<AudioSource>();
    
    musicAudioSource.ignoreListenerPause = true;

    transform.SetParent(null);

    if (Instance != null && Instance != this)
    {
        AudioSource newSource = musicAudioSource; 

        if (newSource.clip == Instance.musicAudioSource.clip)
        {
            Destroy(gameObject);
            return;
        }
        Instance.SetVolume(newSource.volume);
        Instance.PlayNewSong(newSource.clip);
        Destroy(gameObject);
        return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
    
    musicAudioSource.volume = GetMusicVolumeNormalized();
}

    public void PlayNewSong(AudioClip track)
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = track;
        musicAudioSource.Play();
    }

    private void Start()
    {

        float manualVolume = musicAudioSource.volume;
        musicVolume = Mathf.RoundToInt(manualVolume * MUSIC_VOLUME_MAX);
        ResumeMusic();
    }

    private void Update()
    {
        musicTime = musicAudioSource.time;
    }

    public void ChangeMusicVolume()
    {
        musicVolume = (musicVolume + 1) % MUSIC_VOLUME_MAX;
        musicAudioSource.volume = GetMusicVolumeNormalized();
        OnMusicVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetMusicVolumeNormalized()
    {
        return ((float)musicVolume) / MUSIC_VOLUME_MAX;
    }

    public void SetVolume(float volume)
    {
        musicAudioSource.volume = volume;
    
        musicVolume = Mathf.RoundToInt(volume * MUSIC_VOLUME_MAX);
    }

    public void StopMusic()
{
    if (musicAudioSource != null)
    {
        musicAudioSource.Stop();
    }
}

public void ResumeMusic()
{
    if (musicAudioSource != null && !musicAudioSource.isPlaying)
    {
        musicAudioSource.Play();
    }
}
}
