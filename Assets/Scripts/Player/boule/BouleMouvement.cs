using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BouleMouvement : MonoBehaviour
{
    #region public variables

    [Tooltip("Vitesse de rotation de la boule autour du joueur")]
    public float _rotationSpeed = 100.0f; // Vitesse de rotation de la boule autour du joueur
    [Tooltip("Sens de rotation initial")]
    public bool _clockwise = true; // Sens de rotation initial

    [Tooltip("Taille de la boule")]
    public float _size = 1.0f;
    [Tooltip("Vitesse de lancer de la boule")]
    public float _speedThrowing = 10.0f;
    #endregion

    #region Private variables

    private bool _isThrowing = false;
    private Transform _beforeThrow;
    private Transform _player;
    private Vector3 _offset; // Vecteur de d calage initial entre le joueur et la boule
    private Rigidbody _rb;

    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = FindAnyObjectByType<PlayerController>().transform;
    }

    void Start()
    {
        //_player = GetComponentsInParent<Transform>()[1];

        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _player.position.z);
        _offset = (_player.position - this.transform.position) * _size;// Calcule le vecteur de d calage initial entre le joueur et la boule
    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            _isThrowing = true;
            updateThrowing();


        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isThrowing = false;
            //_canReturn = false;
            returnBoule();
        }
        else if(Input.GetMouseButtonDown(0))
        {
            if(_isThrowing)
            {
                _isThrowing = false;
                //_canReturn = false;
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
                //transform.RotateAround(_player.transform.position , Vector3.forward, 50 * Time.deltaTime);
                break;
            case true:
                break;
        }

    }

    private void updateThrowing()
    {
        if(_beforeThrow == null)
        {
            _beforeThrow = this.transform;
        }
        _rb.AddForce(-this.transform.forward * Time.deltaTime * _speedThrowing, ForceMode.VelocityChange);

        //_rb.velocity = -this.transform.forward * Time.deltaTime * _speedThrowing;
        //transform.position += -this.transform.forward * Time.deltaTime * _speedThrowing;
    }

    private void returnBoule()
    {
        this.transform.position = _beforeThrow.position;
        this.transform.rotation = _beforeThrow.rotation;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _beforeThrow = null;
    }
    private void updateRotationBoule()
    {
        
        Quaternion rotation = Quaternion.Euler(0, 0, Time.deltaTime* (_clockwise? _rotationSpeed : -_rotationSpeed));// Calcule la nouvelle position de la boule autour du joueur
        _offset = _offset.normalized * _offset.magnitude; // Normalise l'_offset pour maintenir une distance constante
        _offset = rotation * _offset;
        Vector3 newPosition = _player.position + _offset;
        transform.position = newPosition;// D place la boule   la nouvelle position
        
        transform.LookAt(_player);// Assurez-vous que la boule regarde toujours vers le joueur
    }
    // Fonction appel e lorsque la boule entre en collision avec un autre objet
    private void OnCollisionEnter(Collision collision)
    {
        _clockwise = !_clockwise;// Change le sens de rotation lorsque la collision se produit

    }
}