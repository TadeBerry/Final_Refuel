using UnityEngine;

public class ScorePopupVisualAnimation : MonoBehaviour
{
    private SpriteRenderer backgroundSquare;
    private Color originalColor;
    private float timer = 0f;

    private const float ANIM_DURATION = 0.3f;
    private const float FLASH_DURATION = 0.15f;
    private const float SQUASH_INTENSITY = 1f;

    private void Awake()
    {
        backgroundSquare = GetComponentInChildren<SpriteRenderer>();
        
        if (backgroundSquare != null)
        {
            originalColor = backgroundSquare.color;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer < ANIM_DURATION)
        {
            float progress = timer / ANIM_DURATION;
            float scaleX = Mathf.Lerp(SQUASH_INTENSITY, 1f, progress);
            float scaleY = Mathf.Lerp(0.6f, 1f, progress); 
            
            transform.localScale = new Vector3(scaleX, scaleY, 1f);
        }

        if (timer < FLASH_DURATION && backgroundSquare != null)
        {
            //starts pure white and fades back to original color instantly
            float flashProgress = timer / FLASH_DURATION;
            backgroundSquare.color = Color.Lerp(Color.white, originalColor, flashProgress);
        }
        else if (backgroundSquare != null && timer >= FLASH_DURATION)
        {
            backgroundSquare.color = originalColor;
        }
    }
}