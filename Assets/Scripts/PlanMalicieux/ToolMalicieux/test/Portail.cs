using UnityEngine;

public class Portail : MonoBehaviour
{
    [Range(-1, 1)]
    public int x, y;
    [Range(0, 10)]
    public float distanceAffichage;
    public GameObject otherPortal;
    public bool activate;
    public Portail scriptPortail;
    public bool isTeleporting = false;
    void Start()
    {

        scriptPortail = otherPortal.GetComponent<Portail>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player") && !isTeleporting)
        {
            other.transform.position = otherPortal.transform.position;
            other.GetComponent<PlayerStateMachine>().transform.position = otherPortal.transform.position;
            scriptPortail.isTeleporting = true;
            print("tp"); 
        }
        
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTeleporting = false;
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + x * distanceAffichage, transform.position.y + y * distanceAffichage,0f));
    }

    }
