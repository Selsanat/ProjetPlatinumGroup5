using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouleTestLeandro : MonoBehaviour
{
    public Transform pointstart;
    public float radius;
    public float speed;
    public CharacterController controller;
    private Vector3 dir;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        transform.position = pointstart.position
        //get vector3 forming a circle with radius and speed relative to pointstart
         dir = new Vector3(Mathf.Cos(Time.time*speed), Mathf.Sin(Time.time * speed), 0)*radius;

        controller.Move(dir*Time.deltaTime);

    }
}
