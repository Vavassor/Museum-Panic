using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileSpawner : MonoBehaviour
{
    public List<GameObject> bonePrefabs;
    public Vector3 spawnSize;

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, spawnSize);
    }

    void Start()
    {
        for (int i = 0; i < bonePrefabs.Count; i++)
        {
            GameObject bonePrefab = bonePrefabs[i];
            Vector3 spawnPosition = RandomPointInBox(transform.position, spawnSize);
            spawnPosition.z = transform.position.z;
            Instantiate(bonePrefab, spawnPosition, Random.rotation);
        }
    }

    private static Vector3 RandomPointInBox(Vector3 center, Vector3 size)
    {
        return center + new Vector3(
            (Random.value - 0.5f) * size.x,
            (Random.value - 0.5f) * size.y,
            (Random.value - 0.5f) * size.z
        );
    }
}
