using System.Collections;
using UnityEngine;

public class CoinSparkleAnimation : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SparkleLoop());
    }

    IEnumerator SparkleLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));
            SpawnSparkle();
        }
    }

    void SpawnSparkle()
    {
        Vector3 pos = transform.position + new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f), -0.1f);
        
        GameObject starParent = new GameObject();
        starParent.transform.position = pos;

        for (int i = 0; i < 4; i++)
        {
            GameObject line = GameObject.CreatePrimitive(PrimitiveType.Quad);
            line.transform.SetParent(starParent.transform);
            line.transform.localPosition = Vector3.zero;
            line.transform.localRotation = Quaternion.Euler(0, 0, i * 45f);
            line.transform.localScale = new Vector3(0.25f, 0.05f, 1f);
            
            line.GetComponent<Renderer>().material.color = Color.yellow;
            Destroy(line.GetComponent<Collider>());
        }

        Destroy(starParent, 0.15f);
    }
}