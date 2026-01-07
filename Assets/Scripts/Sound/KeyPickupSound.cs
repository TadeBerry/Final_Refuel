using UnityEngine;

public class KeyPickupSound : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Lander lander))
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayKeyPickupSound();
            }

            Destroy(gameObject);
        }
    }
}