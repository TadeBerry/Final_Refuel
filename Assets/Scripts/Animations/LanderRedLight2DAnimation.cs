using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LanderRedLight2DAnimation : MonoBehaviour
{
    [Header("Pulse Settings")]
    [SerializeField] private float minIntensity = 0.2f;
    [SerializeField] private float maxIntensity = 2.0f;
    [SerializeField] private float speed = 3.0f;

    private Light2D _light2D;

    void Awake()
    {
        _light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        float pulse = Mathf.PingPong(Time.time * speed, 1.0f);
        float smoothPulse = Mathf.SmoothStep(0, 1, pulse);
        _light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, smoothPulse);
    }
}