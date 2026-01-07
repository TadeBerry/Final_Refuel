using UnityEngine;

public class StartPadAudio : MonoBehaviour
{
    private int lastContactCount = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Lander>())
        {
            TriggerSound();
            lastContactCount = collision.contactCount;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Lander>())
        {
            if (collision.contactCount > lastContactCount)
            {
                TriggerSound();
            }
            lastContactCount = collision.contactCount;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Lander>())
        {
            lastContactCount = 0;
        }
    }

    private void TriggerSound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayRandomLandingSound(transform.position);
        }
    }
}