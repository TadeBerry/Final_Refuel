using System.Collections.Generic;
using UnityEngine;

public class FallingStarManager : MonoBehaviour
{
    [SerializeField] private Transform fallingStarPrefab;
    [SerializeField] private List<Transform> spawnPositionList;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnFallingStar), 1f, 0.1f);
    }

    private void SpawnFallingStar()
    {
        if (Random.Range(0, 100) >= 5) return;

        if (spawnPositionList == null || spawnPositionList.Count == 0) return;

        Transform randomSpawnPosition = spawnPositionList[Random.Range(0, spawnPositionList.Count)];

        Vector3 spawnPos = new Vector3(randomSpawnPosition.position.x, randomSpawnPosition.position.y, 0f);
        
        Transform star = Instantiate(fallingStarPrefab, spawnPos, Quaternion.identity);
        
        float randomSize = Random.Range(0.15f, 0.4f);
        star.localScale = Vector3.one * randomSize;

        Rigidbody2D rb = star.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float randomX = Random.Range(-1500f, -800f); 
            float randomY = Random.Range(-1200f, -600f); 
            rb.AddForce(new Vector2(randomX, randomY));
        }

        Destroy(star.gameObject, 8f);
    }   
}