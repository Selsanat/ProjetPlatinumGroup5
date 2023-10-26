using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityEngine;

public class PieceRamassable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Call the function to execute when the object is picked up
            DoSomething();
            
            // Destroy the object
            Destroy(gameObject);
        }
    }
    
    private void DoSomething()
    {
        // Perform the desired action when the object is picked up
        Debug.Log("Object picked up!");
    }
}
