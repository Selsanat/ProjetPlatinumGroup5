using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cameraBackground : MonoBehaviour
{
    public Camera cam;
    private int current = 0;
    public Vector3[] pos; 
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }
    public void moveCamPositiv()
    {
        current++;
        cam.transform.position = pos[current];
    }
    public void moveCamNegativ()
    {
        current--;
        cam.transform.position = pos[current];
    }
    // Update is called once per frame
    
}
