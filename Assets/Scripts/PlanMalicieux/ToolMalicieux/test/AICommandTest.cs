using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public Transform[] points;
    public float speed;

    private int currentPointIndex;
    private Vector3 currentTargetPoint;
    private bool isPlayerOnPlatform;
    private GameObject player;

    private void Start()
    {
        currentPointIndex = 0;
        currentTargetPoint = points[currentPointIndex].position;
        isPlayerOnPlatform = false;
    }

    private void Update()
    {
        if (transform.position == currentTargetPoint)
        {
            if (currentPointIndex < points.Length - 1)
            {
                currentPointIndex++;
            }
            else
            {
                currentPointIndex = 0;
            }

            currentTargetPoint = points[currentPointIndex].position;

            if (isPlayerOnPlatform)
            {
                player.transform.parent = null;
                isPlayerOnPlatform = false;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, currentTargetPoint, speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlayerOnPlatform)
        {
            Debug.Log("Contact Player & Player is not OnPlateform");
            player = collision.gameObject;
            player.transform.SetParent(transform);
            isPlayerOnPlatform = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isPlayerOnPlatform)
        {
            Debug.Log("Contact Player & Player is OnPlateform");
            player.transform.parent = null;
            isPlayerOnPlatform = false;
        }
    }
}
