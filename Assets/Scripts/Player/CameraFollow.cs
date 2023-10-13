using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    void Update()
    {
        if(FindObjectOfType<PlayerStateMachine>()!=null)
        GetComponent<ICinemachineCamera>().Follow = FindObjectOfType<PlayerStateMachine>().transform;
    }
}
