using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine;
using System.Collections;

public class CubeDisco : MonoBehaviour
{
    public Renderer cubeRenderer;
    public Transform cubeTransform;

    public void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        cubeTransform = GetComponent<Transform>();
    }

    public void Update()
    {
        // Change color based on time
        float r = Mathf.Sin(Time.time) * 0.5f + 0.5f;
        float g = Mathf.Sin(Time.time + 2f) * 0.5f + 0.5f;
        float b = Mathf.Sin(Time.time + 4f) * 0.5f + 0.5f;
        cubeRenderer.material.color = new Color(r, g, b);

        // Change size based on time
        float scale = Mathf.Sin(Time.time) * 0.5f + 1f;
        cubeTransform.localScale = new Vector3(scale, scale, scale);

        // Change position based on time
        float posX = Mathf.Sin(Time.time) * 5f;
        float posY = Mathf.Sin(Time.time + 1f) * 2.5f;
        float posZ = Mathf.Sin(Time.time + 2f) * 5f;
        cubeTransform.position = new Vector3(posX, posY, posZ);

        // Change rotation based on time
        float rotX = Mathf.Sin(Time.time) * 180f;
        float rotY = Mathf.Cos(Time.time) * 180f;
        float rotZ = Mathf.Sin(Time.time + 1f) * 180f;
        cubeTransform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
    }
}