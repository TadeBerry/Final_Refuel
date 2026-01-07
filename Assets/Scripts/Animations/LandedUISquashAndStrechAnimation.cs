using System.Collections;
using UnityEngine;

public class LandedUISquashAndStrechAnimation : MonoBehaviour
{
    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void OnEnable()
    {
        StartCoroutine(PopInBounce());
    }

    IEnumerator PopInBounce()
    {
        Vector3 currentScale = transform.localScale;
        Vector3 targetScale;
        

        targetScale = new Vector3(originalScale.x * 1.3f, originalScale.y * 0.7f, originalScale.z);
        for (float t = 0; t < 0.08f; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(currentScale, targetScale, t / 0.08f);
            yield return null;
        }
        transform.localScale = targetScale;
        currentScale = targetScale;
        


        targetScale = new Vector3(originalScale.x * 0.8f, originalScale.y * 1.2f, originalScale.z);
        for (float t = 0; t < 0.08f; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(currentScale, targetScale, t / 0.08f);
            yield return null;
        }
        transform.localScale = targetScale;
        currentScale = targetScale;
        


        targetScale = new Vector3(originalScale.x * 1.05f, originalScale.y * 0.95f, originalScale.z);
        for (float t = 0; t < 0.06f; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(currentScale, targetScale, t / 0.06f);
            yield return null;
        }
        transform.localScale = targetScale;
        currentScale = targetScale;
        

        
        for (float t = 0; t < 0.08f; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(currentScale, originalScale, t / 0.08f);
            yield return null;
        }
        transform.localScale = originalScale;
    }
}