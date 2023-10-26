using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporteur : MonoBehaviour
{
    public Transform[] points;
    private Vector3 _target;
    private int _destPoint; 
    private List<Vector3> _contactPoints = new List<Vector3>();
    public float currentSpeed = 1f;


    private void Update()
    {
        Vector3 dir = (_target - this.transform.position).normalized;
        if (Vector3.Distance(transform.position, _target) < 0.1f)
        {
            _destPoint--;
            _target = _contactPoints[_destPoint];
        }
        transform.Translate(dir * Time.deltaTime * currentSpeed, Space.World);
    }
}
