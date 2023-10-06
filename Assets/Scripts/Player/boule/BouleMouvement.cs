using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouleMouvement : MonoBehaviour
{
    public Transform player; // Référence au joueur (doit être affectée dans l'inspecteur Unity)
    public float rotationSpeed = 5.0f; // Vitesse de rotation de la boule autour du joueur
    public Vector3 rotationAxis = Vector3.left; // Axe de rotation

    public bool clockwise = true; // Sens de rotation initial

    public float size = 1;
    public Vector3 offset; // Vecteur de décalage initial entre le joueur et la boule
    public Vector3 offsetInit; // Vecteur de décalage initial entre le joueur et la boule
    bool isChangingSize = false;
    void Start()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, player.position.z);
        // Calcule le vecteur de décalage initial entre le joueur et la boule
        offset = (player.position - this.transform.position) * size;
        offsetInit = (player.position - this.transform.position) * size;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(size > 1)
                StartCoroutine(sizeDown());
        }
        else if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(agrandissement());
        }
    }

    public IEnumerator agrandissement()
    {
        float currentSize = size;
        isChangingSize = true;
        size += 0.5f;
        while(currentSize <= size)
        {
            
            Quaternion rotation = Quaternion.Euler(0, 0, Time.deltaTime * (clockwise ? rotationSpeed : -rotationSpeed));

            // Normalise l'offset pour maintenir une distance constante
            offset = offset.normalized * offset.magnitude;

            offset = rotation * offset * currentSize;

            Vector3 newPosition = player.position + offset;

            // Déplace la boule à la nouvelle position
            transform.position = newPosition;

            // Assurez-vous que la boule regarde toujours vers le joueur
            transform.LookAt(player);

            currentSize += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        isChangingSize = false;
        yield return null;
    }


    public IEnumerator sizeDown()
    {
        float currentSize = size;
        isChangingSize = true;
        size -= 0.5f;
        while (currentSize >= size)
        {

            Quaternion rotation = Quaternion.Euler(0, 0, Time.deltaTime * (clockwise ? rotationSpeed : -rotationSpeed));

            // Normalise l'offset pour maintenir une distance constante
            offset = offset.normalized * offset.magnitude;

            offset = rotation * offset * currentSize;

            Vector3 newPosition = player.position + offset;

            // Déplace la boule à la nouvelle position
            transform.position = newPosition;

            // Assurez-vous que la boule regarde toujours vers le joueur
            transform.LookAt(player);

            currentSize -= 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        isChangingSize = false;
        yield return null;
    }
    private void FixedUpdate()
    {
        if(!isChangingSize)
        {
            // Calcule la nouvelle position de la boule autour du joueur
            Quaternion rotation = Quaternion.Euler(0, 0, Time.deltaTime * (clockwise ? rotationSpeed : -rotationSpeed));

            // Normalise l'offset pour maintenir une distance constante
            offset = offset.normalized * offset.magnitude;

            offset = rotation * offset;

            Vector3 newPosition = player.position + offset;

            // Déplace la boule à la nouvelle position
            transform.position = newPosition;

            // Assurez-vous que la boule regarde toujours vers le joueur
            transform.LookAt(player);
        }
        
        
    }


    // Fonction appelée lorsque la boule entre en collision avec un autre objet
    private void OnCollisionEnter(Collision collision)
    {
        // Change le sens de rotation lorsque la collision se produit
        clockwise = !clockwise;
    }
}
