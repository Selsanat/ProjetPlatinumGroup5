using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouleMouvement : MonoBehaviour
{
    public Transform _player; // R f rence au joueur (doit  tre affect e dans l'inspecteur Unity)
    [Tooltip("Vitesse de rotation de la boule autour du joueur")]
    public float _rotationSpeed = 100.0f; // Vitesse de rotation de la boule autour du joueur
    [Tooltip("Sens de rotation initial")]
    public bool _clockwise = true; // Sens de rotation initial

    [Tooltip("Taille de la boule")]
    public float _size = 1.0f;
    [Tooltip("Vitesse de lancer de la boule")]
    public float _speedThrowing = 10.0f;

    public Vector3 _offset; // Vecteur de d calage initial entre le joueur et la boule

    #region Private variables
    private bool _isThrowing = false;
    private bool _canReturn = true; // a mettre en false de base
    private Transform _beforeThrow;
    #endregion

    void Start()
    {
       _player = GetComponentsInParent<Transform>()[1];
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _player.position.z);
        // Calcule le vecteur de d calage initial entre le joueur et la boule
        _offset = (_player.position - this.transform.position) * _size;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _isThrowing = true;

        }
        else if(Input.GetMouseButtonDown(0))
        {
            if(_isThrowing && _canReturn)
            {
                _isThrowing = false;
                _canReturn = false;
                returnBoule();
            }
        }
    }

   
    private void FixedUpdate()
    {

        switch (_isThrowing)
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
        if(_beforeThrow == null)
        {
            _beforeThrow = this.transform;
        }
        Debug.Log(this.transform.localRotation.y);
        //if ((_clockwise && this.transform.rotation.y == 90) || (!_clockwise && this.transform.rotation.y == -90)) //avec le look at, la rotation peut switch de sens, il faut donc changé le inversé le up 

        if (_clockwise && this.transform.rotation.y == 90) //avec le look at, la rotation peut switch de sens, il faut donc changé le inversé le up 
        {
            transform.position += -this.transform.up * Time.deltaTime * _speedThrowing;
            Debug.Log("test");
        }
        else if (!_clockwise && this.transform.rotation.y == -90)
        {
            transform.position += -this.transform.up * Time.deltaTime * _speedThrowing;
            Debug.Log("test2");
        }
        else
        {
            transform.position += this.transform.up * Time.deltaTime * _speedThrowing;
            Debug.Log("else");
        }

    }

    private void returnBoule()
    {
        this.transform.position = _beforeThrow.position;
        this.transform.rotation = _beforeThrow.rotation;
        _beforeThrow = null;
    }
    private void updateRotationBoule()
    {
        // Calcule la nouvelle position de la boule autour du joueur
        Quaternion rotation = Quaternion.Euler(0, 0, Time.deltaTime* (_clockwise? _rotationSpeed : -_rotationSpeed));

        // Normalise l'_offset pour maintenir une distance constante
        _offset = _offset.normalized* _offset.magnitude;

        _offset = rotation * _offset;

        Vector3 newPosition = _player.position + _offset;

        // D place la boule   la nouvelle position
        transform.position = newPosition;

        // Assurez-vous que la boule regarde toujours vers le joueur
        transform.LookAt(_player);
    }
    // Fonction appel e lorsque la boule entre en collision avec un autre objet
    private void OnCollisionEnter(Collision collision)
    {
        // Change le sens de rotation lorsque la collision se produit
        _clockwise = !_clockwise;
        if(_isThrowing)
        {
            _canReturn = true;
        }
        
    }
}