using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteract : MonoBehaviour
{
    public float timeSlowPercentage;
    public float timeSlowDuration;
    public float timeAccelerationPercentage;
    public float timeAccelerationDuration;

    private bool playerEntered;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerEntered = true;
        }
    }

    private void Update()
    {
        if (playerEntered)
        {
            PlayerStateMachine player = GameObject.FindObjectOfType<PlayerStateMachine>();
            if (player.velocity.x > 0)
            {
                Debug.Log("Superieur");
                Time.timeScale = 1 - (timeSlowPercentage / 100);
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                StartCoroutine(ResetTime());
            }
            else if (player.velocity.x < 0)
            {
                Debug.Log("inferieur");
                Time.timeScale = 1 + (timeAccelerationPercentage / 100);
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                StartCoroutine(ResetTime());
            }
        }
    }

    private IEnumerator ResetTime()
    {
        yield return new WaitForSecondsRealtime(timeSlowDuration);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        playerEntered = false;
    }
}