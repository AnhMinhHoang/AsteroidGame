using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid AsteroidPrefab;
    [Range(0f, 45f)]
    public float TrajectoryVariance = 15.0f;
    public float SpawnRate = 2.0f;
    public float SpawnDistance = 15.0f;
    public int SpawnAmount = 1;
    private void Start()
    {
        InvokeRepeating(nameof(Spawn), SpawnRate, SpawnRate);
    }

    private void Spawn()
    {
        for (int i = 0; i < SpawnAmount; i++)
        {
            //Random direction using normalize
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * SpawnDistance;
            Vector3 spawnPoint = transform.position + spawnDirection;

            //Trajectory change
            float variance = Random.Range(-TrajectoryVariance, TrajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            Asteroid asteroid = Instantiate(AsteroidPrefab, spawnPoint, rotation);
            asteroid.Size = Random.Range(asteroid.MinSize, asteroid.MaxSize);
            asteroid.SetTrajectory(rotation * -spawnDirection);
        }
    }
}
