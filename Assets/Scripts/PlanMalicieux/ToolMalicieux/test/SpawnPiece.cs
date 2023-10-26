using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Vector2 spawnAreax, spawnAreay;
    public float spawnTime, Z;
    private float decrementation;

    private void Start()
    {
        decrementation = spawnTime;
        //SpawnRandomObject();
    }
    private void Update()
    {
        decrementation = decrementation - Time.deltaTime;
        if (decrementation <= 0)
        {
            SpawnRandomObject();
            Destroy(gameObject);
        }
    }
    private void SpawnRandomObject()
    {
        float randomX = Random.Range(spawnAreax.x, spawnAreax.y);
        float randomY = Random.Range(spawnAreay.x, spawnAreay.y);
        
        Vector3 spawnPosition = new Vector3(randomX, randomY, Z);
        
        GameObject newObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }
}
