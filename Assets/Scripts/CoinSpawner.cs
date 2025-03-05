using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private Coins coinPrefab;
    [SerializeField] private float minSpawnRate = 5f;
    [SerializeField] private float maxSpawnRate = 15f;
    [SerializeField] private int spawnAmount = 1;

    private void Start()
    {
        StartCoroutine("SpawnCoroutine");
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            float randomDelay = Random.Range(minSpawnRate, maxSpawnRate);
            yield return new WaitForSeconds(randomDelay);
            Spawn();
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            float x = Random.Range(-8.5f, 8.5f);
            float y = Random.Range(-4.5f, 4.5f);
            Vector3 spawnPoint = new Vector3(x, y, 0);

            Instantiate(coinPrefab, spawnPoint, Quaternion.identity);
        }
    }
}
