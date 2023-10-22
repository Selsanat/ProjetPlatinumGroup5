using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityEngine;

public class AdjustablePlatform : MonoBehaviour
{
    public float speed = 5f;
    public float maxDistance = 10f;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingForward = true;

    private void Start()
    {
        startPos = transform.position;
        endPos = startPos + transform.right * maxDistance;
    }

    private void Update()
    {
        if (movingForward)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
            if (transform.position == endPos)
            {
                movingForward = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
            if (transform.position == startPos)
            {
                movingForward = true;
            }
        }
    }
}
