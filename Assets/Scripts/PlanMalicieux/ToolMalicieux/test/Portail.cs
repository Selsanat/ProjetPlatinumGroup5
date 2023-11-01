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
            print(other.transform.position);
            scriptPortail.isTeleporting = true;
            other.transform.position = new Vector3(otherPortal.transform.position.x, otherPortal.transform.position.y, other.transform.position.z);
            other.GetComponent<PlayerStateMachine>().transform.position = new Vector3(otherPortal.transform.position.x, otherPortal.transform.position.y, other.transform.position.z);
            print(other.transform.position);
            print("tp"); 
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTeleporting = false;
            print("exit");
        }
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + x * distanceAffichage, transform.position.y + y * distanceAffichage,0f));
    }

}
