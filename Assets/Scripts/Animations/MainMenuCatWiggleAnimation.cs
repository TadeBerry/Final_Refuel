using UnityEngine;

public class MainMenuCatWiggleAnimation : MonoBehaviour
{
    private float wiggleAmount = 10f;
    private float wiggleSpeed = 2f;
    private float startRotation;
    private float randomOffset;


    void Start()
    {
        startRotation = transform.rotation.eulerAngles.z;

        wiggleAmount = Random.Range(wiggleAmount * 0.5f, wiggleAmount * 1f);
        wiggleSpeed = Random.Range(wiggleSpeed * 0.7f, wiggleSpeed * 1.3f);
    }

    void Update()
    {

        float wiggle = Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount;
        

        transform.rotation = Quaternion.Euler(0, 0, wiggle);
    }
}