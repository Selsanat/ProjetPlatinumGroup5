using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouleMouvement : MonoBehaviour
{
    public Transform player; // R f rence au joueur (doit  tre affect e dans l'inspecteur Unity)

    [Tooltip("Vitesse de rotation de la boule autour du joueur")]
    public float rotationSpeed = 5.0f; // Vitesse de rotation de la boule autour du joueur
    public Vector3 rotationAxis = Vector3.left; // Axe de rotation
    [Tooltip("Sens de rotation initial")]
    public bool clockwise = true; // Sens de rotation initial

    [Tooltip("Taille de la boule")]
    public float size = 1;
    [Tooltip("Vitesse de lancer de la boule")]
    public float speedThrowing = 1;

    public Vector3 offset; // Vecteur de d calage initial entre le joueur et la boule
    private float angle = 0;
    private bool isThrowing = false;

    void Start()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, player.position.z);
        // Calcule le vecteur de d calage initial entre le joueur et la boule
        offset = (player.position - this.transform.position) * size;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isThrowing = true;
        }
    }

   
    private void FixedUpdate()
    {

        switch (isThrowing)
        {
            case false:
                updateRotationBoule();
                break;
            case true:
                updateThrowing();
                break;
            
        }


    }

    private void updateThrowing()
    {

        // D place la boule   la nouvelle position
        transform.position += transform.InverseTransformDirection(Vector3.forward) * Time.deltaTime * speedThrowing;
        
    }
    private void updateRotationBoule()
    {
        // Calcule la nouvelle position de la boule autour du joueur
        Quaternion rotation = Quaternion.Euler(0, 0, Time.deltaTime* (clockwise? rotationSpeed : -rotationSpeed));

        // Normalise l'offset pour maintenir une distance constante
        offset = offset.normalized* offset.magnitude;

        offset = rotation * offset;

        Vector3 newPosition = player.position + offset;

        // D place la boule   la nouvelle position
        transform.position = newPosition;

        // Assurez-vous que la boule regarde toujours vers le joueur
        transform.LookAt(player);
    }
    // Fonction appel e lorsque la boule entre en collision avec un autre objet
    private void OnCollisionEnter(Collision collision)
    {
        // Change le sens de rotation lorsque la collision se produit
        clockwise = !clockwise;
    }
}