using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Teleporteur : MonoBehaviour
{
    public GameObject _teleporteur1, _teleporteur2;
    bool isTeleporting = false;
    // Start is called before the first frame update

    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !isTeleporting)
        {
            isTeleporting = true;
            if (collision.gameObject.transform.position == _teleporteur1.transform.position)
                collision.transform.position = _teleporteur1.transform.position;
            else if (collision.gameObject.transform.position == _teleporteur2.transform.position)
                collision.transform.position = _teleporteur2.transform.position;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTeleporting = false;
        }
    }


}
