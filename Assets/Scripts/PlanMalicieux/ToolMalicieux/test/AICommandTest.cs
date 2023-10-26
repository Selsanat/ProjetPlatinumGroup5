using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityEngine;

public class SpawnPiece : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Vector2 spawnArea;
    public float spawnTime, decrementation;

    private void Start()
    {
        //SpawnRandomObject();
    }
    private void Update()
    {
        decrementation = decrementation - Time.deltaTime;
        if (decrementation <= spawnTime)
        {
            SpawnRandomObject();
        }
    }
    private void SpawnRandomObject()
    {
        float randomX = Random.Range(-spawnArea.x, spawnArea.x);
        float randomY = Random.Range(-spawnArea.y, spawnArea.y);
        
        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);
        
        GameObject newObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }
}
