using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ThrusterCracklingRemover : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 20f; // Higher = faster fade
    
    private AudioSource audioSource;
    private float targetVolume;
    private float maxVolume;
    private bool shouldBePlaying;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        maxVolume = audioSource.volume;
        audioSource.volume = 0f;
    }

    private void Start()
    {
        if (audioSource.playOnAwake || audioSource.loop)
        {
            audioSource.Play();
        }
    }

    private void Update()
    {
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, fadeSpeed * Time.deltaTime);
    }

    public void FadeIn()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        shouldBePlaying = true;
        targetVolume = maxVolume;
    }

    public void FadeOut()
    {
        shouldBePlaying = false;
        targetVolume = 0f;
    }

    public void SetMaxVolume(float volume)
    {
        maxVolume = volume;
        if (shouldBePlaying)
        {
            targetVolume = maxVolume;
        }
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying && targetVolume > 0f;
    }
}