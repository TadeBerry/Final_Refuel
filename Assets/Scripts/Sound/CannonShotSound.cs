using UnityEngine;

public class CannonShotSound : MonoBehaviour
{
    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayCannonShotSound(transform.position);
        }
    }
}