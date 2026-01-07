using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private bool isShaking = false;
    private float shakeDuration = 0f;
    private float shakeIntensity = 0f;
    private Vector3 originalCameraPos;

    void Start()
    {
        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        if (e.landingType != Lander.LandingType.Success)
        {
            StartShake(0.5f, 0.3f);
        }
    }

    public void StartShake(float duration, float intensity)
    {
        shakeDuration = duration;
        shakeIntensity = intensity;
        isShaking = true;
        originalCameraPos = Camera.main.transform.position;
    }

    void LateUpdate()
    {
        if (isShaking)
        {
            if (shakeDuration > 0)
            {
                Camera.main.transform.position = originalCameraPos + Random.insideUnitSphere * shakeIntensity;
                shakeDuration -= Time.deltaTime;
            }
            else
            {
                isShaking = false;
                shakeDuration = 0f;
                Camera.main.transform.position = originalCameraPos;
            }
        }
        else if (!isShaking)
        {
            originalCameraPos = Camera.main.transform.position;
        }
    }
}