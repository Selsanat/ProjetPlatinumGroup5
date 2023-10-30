using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
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
            if (GameObject.FindObjectOfType<PlayerStateMachine>().velocity.x > 0)
            {
                print("Superieur");
                Time.timeScale = 1 - (timeSlowPercentage / 100);
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                StartCoroutine(ResetTime());
            }
            else if (GameObject.FindObjectOfType<PlayerStateMachine>().velocity.x < 0)
            {
                print("inferieur");
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

public class LeverController : MonoBehaviour
{
    private List<Lever> levers;

    private void Start()
    {
        levers = new List<Lever>();
        Lever[] leverObjects = FindObjectsOfType<Lever>();
        foreach (Lever lever in leverObjects)
        {
            levers.Add(lever);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Lever lever in levers)
            {
                lever.timeSlowDuration += 1f;
                lever.timeAccelerationDuration += 1f;
            }
        }
    }
}
