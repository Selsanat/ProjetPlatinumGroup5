using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilloneV2 : MonoBehaviour
{
    public float speed = 5f;
    public Transform[] pillars;
    public float waitTime = 2f;

    private int currentPillarIndex = 0;
    private Transform currentPillar;
    private bool isMoving = true;

    private void Start()
    {
        if (pillars.Length == 0)
        {
            Debug.LogError("No pillars assigned to the MovingBall script.");
            return;
        }

        currentPillar = pillars[currentPillarIndex];
        StartCoroutine(MoveToPoint(currentPillar.position));
    }

    private void Update()
    {
        if (!isMoving)
            return;

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentPillar.position, step);

        if (transform.position == currentPillar.position)
        {
            StartCoroutine(WaitBeforeNextMove());
        }
    }

    private IEnumerator MoveToPoint(Vector3 target)
    {
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator WaitBeforeNextMove()
    {
        isMoving = false;
        yield return new WaitForSeconds(waitTime);

        currentPillarIndex = (currentPillarIndex + 1) % pillars.Length;
        currentPillar = pillars[currentPillarIndex];

        StartCoroutine(MoveToPoint(currentPillar.position));
        isMoving = true;
    }
}
