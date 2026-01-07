using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thrusterAudioSource;
    
    private Lander lander;
    private bool wasThrusting = false;
    private ThrusterCracklingRemover cracklingRemover;

    private void Awake()
    {
        lander = GetComponent<Lander>();
        cracklingRemover = thrusterAudioSource.GetComponent<ThrusterCracklingRemover>();
    }

    private void Start()
    {
        SoundManager.Instance.OnSoundVolumeChanged += SoundManager_OnSoundVolumeChanged;
        
        thrusterAudioSource.enabled = true;
        thrusterAudioSource.loop = true;
        thrusterAudioSource.volume = SoundManager.Instance.GetSoundVolumeNormalized();
        
        lander.OnStateChanged += Lander_OnStateChanged;
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        if (e.state != Lander.State.Normal)
        {
            cracklingRemover.FadeOut();
            wasThrusting = false;
        }
    }

    private void SoundManager_OnSoundVolumeChanged(object sender, System.EventArgs e)
    {
        cracklingRemover.SetMaxVolume(SoundManager.Instance.GetSoundVolumeNormalized());
    }

    private void Update()
    {
        if (lander.GetState() != Lander.State.Normal || lander.GetFuel() <= 0f)
        {
            if (wasThrusting)
            {
                cracklingRemover.FadeOut();
                wasThrusting = false;
            }
            return;
        }

        float gamepadDeadzone = 0.4f;
        bool isThrusting = GameInput.Instance.IsUpActionPressed() || 
                          GameInput.Instance.IsLeftActionPressed() || 
                          GameInput.Instance.IsRightActionPressed() ||
                          GameInput.Instance.GetMovementInputVector2().magnitude > gamepadDeadzone;

        if (isThrusting != wasThrusting)
        {
            if (isThrusting)
            {
                cracklingRemover.FadeIn();
            }
            else
            {
                cracklingRemover.FadeOut();
            }
            wasThrusting = isThrusting;
        }
    }

    private void OnDestroy()
    {
        if (lander != null)
        {
            lander.OnStateChanged -= Lander_OnStateChanged;
        }
        
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.OnSoundVolumeChanged -= SoundManager_OnSoundVolumeChanged;
        }
    }
}