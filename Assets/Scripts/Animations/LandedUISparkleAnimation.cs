using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LandedUISparkleAnimation : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(CreateSparkles());
    }

    IEnumerator CreateSparkles()
    {
        yield return new WaitForSeconds(0.1f);
        
        float totalTime = 1.2f;
        float elapsed = 0f;
        
        while (elapsed < totalTime)
        {
            float wait = Random.Range(0.05f, 0.15f);
            CreateSparkle();
            yield return new WaitForSeconds(wait);
            elapsed += wait;
        }
    }

    void CreateSparkle()
    {
        GameObject sparkle = new GameObject();
        sparkle.transform.SetParent(transform, false);
        
        Image img = sparkle.AddComponent<Image>();
        img.color = Color.yellow;
        
        RectTransform rt = sparkle.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(Random.Range(-500f, 500f), Random.Range(-300f, 300f));
        rt.sizeDelta = new Vector2(Random.Range(10f, 20f), Random.Range(10f, 20f));
        rt.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        
        StartCoroutine(AnimateSparkle(sparkle, rt, img));
        
        Destroy(sparkle, 1f);
    }

    IEnumerator AnimateSparkle(GameObject sparkle, RectTransform rt, Image img)
    {
        Vector2 startSize = rt.sizeDelta;
        float time = 0f;
        float duration = 0.7f;
        
        while (time < duration)
        {
            if (sparkle == null) yield break;

            time += Time.deltaTime;
            float t = time / duration;
            
            rt.sizeDelta = startSize * (1f - t);
            Color c = img.color;
            c.a = 1f - t;
            img.color = c;
            
            yield return null;
        }
    }
}