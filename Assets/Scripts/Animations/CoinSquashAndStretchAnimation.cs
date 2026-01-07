using System.Collections;
using UnityEngine;

public class CoinSquashAndStretchAnimation : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(AnimationLoop());
    }

    IEnumerator AnimationLoop()
    {
        while (true)
        {
            // Wait randomly (3 to 8 seconds)
            yield return new WaitForSeconds(Random.Range(3f, 8f));
            
            yield return StartCoroutine(SquashAndStretch());
        }
    }

    IEnumerator SquashAndStretch()
    {
        // Squash down (wider, shorter)
        transform.localScale = new Vector3(1.2f, 0.8f, 1f);
        yield return new WaitForSeconds(0.05f);
        
        transform.localScale = new Vector3(1.3f, 0.7f, 1f);
        yield return new WaitForSeconds(0.05f);
        
        //back to normal
        transform.localScale = new Vector3(1.1f, 0.9f, 1f);
        yield return new WaitForSeconds(0.05f);
        
        transform.localScale = new Vector3(0.9f, 1.1f, 1f);
        yield return new WaitForSeconds(0.05f);
        
        transform.localScale = new Vector3(0.8f, 1.2f, 1f);
        yield return new WaitForSeconds(0.05f);
        
        //back to normal
        transform.localScale = new Vector3(0.9f, 1.05f, 1f);
        yield return new WaitForSeconds(0.05f);
        
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
}