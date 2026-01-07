using UnityEngine;

public class ScorePopupDisappearAnimation : MonoBehaviour
{
    // Wait for the popup to be readable before zooming out
    private const float DISAPPEAR_DELAY = 0.8f; 
    private const float ZOOM_SPEED = 6f;
    
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= DISAPPEAR_DELAY)
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale, 
                Vector3.zero, 
                Time.deltaTime * ZOOM_SPEED
            );

            if (transform.localScale.x < 0.01f)
            {
                transform.localScale = Vector3.zero;
                enabled = false; 
            }
        }
    }
}