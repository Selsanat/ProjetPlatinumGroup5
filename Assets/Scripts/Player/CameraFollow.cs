using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 center;
    private float dist;
    void Update()
    {

        if (FindObjectOfType<PlayerStateMachine>() != null)
        {
            dist = 0;
            GetComponent<Camera>().orthographicSize = 0;
            center = Vector3.zero;  
            PlayerStateMachine[] players = FindObjectsOfType<PlayerStateMachine>();
            foreach (PlayerStateMachine player in players)
            {
                center += player.transform.position;
                dist += Vector3.Distance(player.transform.position, players[0].transform.position);
            }

            dist = dist / players.Length;
            dist = Mathf.Clamp(dist, 3, 9999);
            transform.position = new Vector3(center.x/ players.Length, center.y/ players.Length);
            GetComponent<Camera>().orthographicSize = dist;
        }

    }
}
