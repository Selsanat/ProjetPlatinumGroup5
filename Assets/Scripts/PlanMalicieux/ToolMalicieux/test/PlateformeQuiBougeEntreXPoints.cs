using UnityEngine;

public class PlateformeQuiBougeEntreXPoints : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5f;

    private int currentWaypointIndex = 0;

    private void Update()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Vector3 targetPosition = waypoints[currentWaypointIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                currentWaypointIndex++;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}