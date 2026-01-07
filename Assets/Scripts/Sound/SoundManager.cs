using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    

    private const int SOUND_VOLUME_MAX = 10;


    public static SoundManager Instance { get; private set; }

    private static int soundVolume = 5;


    public event EventHandler OnSoundVolumeChanged;

    [SerializeField] private AudioClip fuelPickupAudioClip;
    [SerializeField] private AudioClip coinPickupAudioClip;
    [SerializeField] private AudioClip crashAudioClip;
    [SerializeField] private AudioClip landingSuccessAudioClip;
    [SerializeField] private AudioClip rockHitAudioClip;
    [SerializeField] private AudioClip buttonHoverAudioClip;
    [SerializeField] private AudioClip keyPickupAudioClip;
    [SerializeField] private AudioClip cannonShotAudioClip;

    [SerializeField] private AudioClip landingSound1;
    [SerializeField] private AudioClip landingSound2;
    [SerializeField] private AudioClip landingSound3;
    [SerializeField] private AudioClip landingSound4;
    [SerializeField] private AudioClip landingSound5;
    [SerializeField] private AudioClip landingSound6;

    private AudioSource uiAudioSource;


    private void Awake() 
    {
        transform.SetParent(null); 

        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
            return;  
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 


        uiAudioSource = gameObject.AddComponent<AudioSource>();
        uiAudioSource.ignoreListenerPause = true;
        uiAudioSource.playOnAwake = false;
    }


    private void Start()
    {
        SubscribeToLanderEvents();
    }
    public void SubscribeToLanderEvents()
    {
        if (Lander.Instance != null)
        {
            Lander.Instance.OnFuelPickup -= Lander_OnFuelPickup;
            Lander.Instance.OnCoinPickup -= Lander_OnCoinPickup;
            Lander.Instance.OnLanded -= Lander_OnLanded;

            Lander.Instance.OnFuelPickup += Lander_OnFuelPickup;
            Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
            Lander.Instance.OnLanded += Lander_OnLanded;
        }
    }


     private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.Success:
                if (MusicManager.Instance != null)
            {
                MusicManager.Instance.PlayNewSong(landingSuccessAudioClip);
                
                MusicManager.Instance.SetVolume(GetSoundVolumeNormalized() * 0.3f);
            }
                break;
            default:
                AudioSource.PlayClipAtPoint(crashAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
            break;
        }
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinPickupAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
    }

    private void Lander_OnFuelPickup(object sender, System.EventArgs e)
    {
        float fuelVolume = GetSoundVolumeNormalized() * 0.4f;
        AudioSource.PlayClipAtPoint(fuelPickupAudioClip, Camera.main.transform.position, fuelVolume);
    }

    public void PlayRockHitSound()
    {
    AudioSource.PlayClipAtPoint(rockHitAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized() * 0.2f);
    }

    public void PlayButtonHoverSound()
    {
        if (buttonHoverAudioClip != null && uiAudioSource != null)
        {
        uiAudioSource.PlayOneShot(buttonHoverAudioClip, GetSoundVolumeNormalized() * 0.5f);
        }
    }

    public void PlayRandomLandingSound(Vector3 position)
    {
    int randomIndex = UnityEngine.Random.Range(1, 7);
    AudioClip selectedClip = null;

    if (randomIndex == 1) selectedClip = landingSound1;
    else if (randomIndex == 2) selectedClip = landingSound2;
    else if (randomIndex == 3) selectedClip = landingSound3;
    else if (randomIndex == 4) selectedClip = landingSound4;
    else if (randomIndex == 5) selectedClip = landingSound5;
    else if (randomIndex == 6) selectedClip = landingSound6;

    if (selectedClip != null)
    {
        AudioSource.PlayClipAtPoint(selectedClip, position, GetSoundVolumeNormalized() * 2f);
    }
}

    public void PlayKeyPickupSound()
    {
    if (keyPickupAudioClip != null)
    {
        AudioSource.PlayClipAtPoint(keyPickupAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
    }
    }


    public void PlayCannonShotSound(Vector3 position)
{
    if (cannonShotAudioClip != null)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;
        AudioSource source = tempGO.AddComponent<AudioSource>();

        source.clip = cannonShotAudioClip;
        source.volume = GetSoundVolumeNormalized();
        source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        source.spatialBlend = 0f;

        source.Play();
        Destroy(tempGO, cannonShotAudioClip.length);
    }
}


    public void ChangeSoundVolume()
    {
        soundVolume = (soundVolume + 1) % SOUND_VOLUME_MAX;
        OnSoundVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetSoundVolume()
    {
        return soundVolume;
    }

    public float GetSoundVolumeNormalized()
    {
        return ((float)soundVolume) / SOUND_VOLUME_MAX;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SubscribeToLanderEvents();
    }

}
