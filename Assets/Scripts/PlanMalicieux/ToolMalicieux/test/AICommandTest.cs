using UnityEngine;

public class Script : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("B"))
        {
            other.gameObject.transform.position = new Vector3(
                other.gameObject.transform.position.x,
                other.gameObject.transform.position.y - 1,
                other.gameObject.transform.position.z
            );
        }
    }
}
