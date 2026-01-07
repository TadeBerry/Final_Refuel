using UnityEngine;
using UnityEngine.UI;

public class MainMenuLogoSparklingAnimation : MonoBehaviour
{
    public Image logoImage;
    public float shimmerSpeed = 2f;
    public float minBrightness = 0.9f;
    public float maxBrightness = 1f;

    private Color originalColor;

    void Start()
{
    if (logoImage == null) logoImage = GetComponent<Image>();
    
    originalColor = logoImage.color;
}

    void Update()
    {
        float noise = Mathf.PingPong(Time.time * shimmerSpeed, 1.0f);
        float brightness = Mathf.Lerp(minBrightness, maxBrightness, noise);
        
        logoImage.color = new Color(
            originalColor.r * brightness,
            originalColor.g * brightness,
            originalColor.b * brightness,
            originalColor.a
        );
    }
}