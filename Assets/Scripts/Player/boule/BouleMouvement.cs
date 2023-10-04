using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouleMouvement : MonoBehaviour
{
    public Transform player; // R�f�rence au joueur (doit �tre affect�e dans l'inspecteur Unity)
    public float rotationSpeed = 5.0f; // Vitesse de rotation de la boule autour du joueur
    public Vector3 rotationAxis = Vector3.left; // Axe de rotation

    public bool clockwise = true; // Sens de rotation initial


    private Vector3 offset; // Vecteur de d�calage initial entre le joueur et la boule

    void Start()
    {
        // Calcule le vecteur de d�calage initial entre le joueur et la boule
        offset = transform.position - player.position;
    }

    void Update()
    {
        // Calcule la nouvelle position de la boule autour du joueur
        Quaternion rotation = Quaternion.Euler(0, 0, Time.time * (clockwise == false ? rotationSpeed : -rotationSpeed));


        Vector3 newPosition = player.position + rotation * offset;

        // D�place la boule � la nouvelle position
        transform.position = newPosition;

        // Assurez-vous que la boule regarde toujours vers le joueur
        transform.LookAt(player);
    }

    // Fonction appel�e lorsque la boule entre en collision avec un autre objet
    private void OnCollisionEnter(Collision collision)
    {
        // Change le sens de rotation lorsque la collision se produit
        clockwise = !clockwise;
    }
}
