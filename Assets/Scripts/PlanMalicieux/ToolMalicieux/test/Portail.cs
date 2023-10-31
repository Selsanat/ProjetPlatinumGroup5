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
    void Start()
    {

        scriptPortail = otherPortal.GetComponent<Portail>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
            if (other.CompareTag("Player"))
            {
                other.transform.position = new Vector3(otherPortal.transform.position.x + scriptPortail.x * scriptPortail.distanceAffichage, otherPortal.transform.position.y + scriptPortail.y * scriptPortail.distanceAffichage, other.transform.position.z);                
            }
        
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + x * distanceAffichage, transform.position.y + y * distanceAffichage,0f));
    }

    }
