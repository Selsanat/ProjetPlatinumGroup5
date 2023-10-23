using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityEngine;

public class Script : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 5f;
    private Vector3 startPos;
    private float direction = 1f;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float newX = transform.position.x + direction * speed * Time.deltaTime;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        if (Mathf.Abs(transform.position.x - startPos.x) >= distance)
        {
            direction *= -1f;
        }
    }
}
