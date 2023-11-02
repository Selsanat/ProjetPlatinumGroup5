using UnityEngine;

public class Portail : MonoBehaviour
{
    [Header("Mettre l'autre portail")]
    [Space(5)]
    public GameObject otherPortal;
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
            scriptPortail.isTeleporting = true;
            other.transform.position = new Vector3(otherPortal.transform.position.x, otherPortal.transform.position.y, other.transform.position.z);

            print("tp"); 
        }
        
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            isTeleporting = false;
            print("out");
        }
    }


}
