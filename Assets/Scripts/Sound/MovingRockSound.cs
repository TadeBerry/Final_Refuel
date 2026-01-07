using UnityEngine;

public class MovingRockSound : MonoBehaviour
{
    [SerializeField] private float hitForceThreshold = 2f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactSpeed = collision.relativeVelocity.magnitude;

        if (impactSpeed > hitForceThreshold)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayRockHitSound();
            }
        }
    }
}