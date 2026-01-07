using UnityEngine;

public class LowFuelAnimation : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float timer;

    private const float PULSE_SPEED = 6f; 
    private const float MIN_ALPHA = 0.2f;
    private const float MAX_ALPHA = 0.5f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void Update()
    {
        timer += Time.deltaTime * PULSE_SPEED;

        float lerpFactor = (Mathf.Sin(timer) + 1f) / 2f;

        canvasGroup.alpha = Mathf.Lerp(MIN_ALPHA, MAX_ALPHA, lerpFactor);
    }
}