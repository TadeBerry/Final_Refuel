using UnityEngine;

public class MainMenuMenuSquishSquashAnimation : MonoBehaviour
{
    public float squishSpeed = 2.5f;     //bounce
    public float squishIntensity = 0.1f; //deform
    public float rotateSpeed = 1.2f;     //tilt
    public float rotateIntensity = 2.0f; //degree of tilt

    private Vector3 initialScale;
    private Quaternion initialRotation;

    void Start()
    {
        initialScale = transform.localScale;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        float horizontalSquish = Mathf.Sin(Time.time * squishSpeed) * squishIntensity;

        transform.localScale = new Vector3(
            initialScale.x + horizontalSquish, 
            initialScale.y - horizontalSquish, 
            initialScale.z
        );

        float tilt = Mathf.Sin(Time.time * rotateSpeed) * rotateIntensity;
        transform.localRotation = initialRotation * Quaternion.Euler(0, 0, tilt);
    }
}