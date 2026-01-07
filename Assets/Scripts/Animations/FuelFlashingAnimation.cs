using UnityEngine;

public class FlashAnimation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private float nextFlashTime;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        
        //first flash
        nextFlashTime = Time.time + Random.Range(2f, 5f);
    }

    void Update()
    {
        //check if it's time to flash
        if (Time.time >= nextFlashTime)
        {
            StartCoroutine(Flash());
            nextFlashTime = Time.time + Random.Range(5f, 15f);
        }
    }

    System.Collections.IEnumerator Flash()
    {

        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = new Color(1.5f, 1.5f, 1.5f, 1f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
}