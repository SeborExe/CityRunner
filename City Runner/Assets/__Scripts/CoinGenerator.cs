using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int spawnVariant;
    [SerializeField] private int coinSpawnChancePercent;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        int cointToSpawn = 5;

        for (int i = 0; i < cointToSpawn; i++)
        {
            spawnVariant++;

            if (Random.Range(1, 100) <= coinSpawnChancePercent)
            {
                Instantiate(coinPrefab, new Vector3(transform.position.x + spawnVariant, transform.position.y, transform.position.z), Quaternion.identity);  
            }
        }
    }
}
