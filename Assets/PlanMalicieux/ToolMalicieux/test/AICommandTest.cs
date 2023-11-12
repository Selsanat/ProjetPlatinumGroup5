using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public GameObject laserPrefab; // The laser prefab to be instantiated
    public Transform pointA; // The point A where the laser starts
    public Transform pointB; // The point B where the laser ends

    public float minTimeBetweenShots = 1f; // The minimum time between laser shots
    public float maxTimeBetweenShots = 3f; // The maximum time between laser shots
    public float minLaserDuration = 0.5f; // The minimum duration of the laser
    public float maxLaserDuration = 2f; // The maximum duration of the laser

    private bool isLaserActive = false; // Flag to check if the laser is currently active

    private void Start()
    {
        StartCoroutine(ShootLaser());
    }

    IEnumerator ShootLaser()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenShots, maxTimeBetweenShots));

            ActivateLaser();

            yield return new WaitForSeconds(Random.Range(minLaserDuration, maxLaserDuration));

            DeactivateLaser();
        }
    }

    void ActivateLaser()
    {
        if (!isLaserActive)
        {
            GameObject laserInstance = Instantiate(laserPrefab, pointA.position, Quaternion.identity);
            laserInstance.transform.LookAt(pointB);
            isLaserActive = true;
        }
    }

    void DeactivateLaser()
    {
        if (isLaserActive)
        {
            Destroy(GameObject.FindGameObjectWithTag("Laser"));
            isLaserActive = false;
        }
    }
}