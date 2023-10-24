using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

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
    public float _speedBack = 2.0f;
    public PhysicMaterial _bounce;
    #endregion

    #region Private variables

    private bool _isThrowing = false;
    private Transform _beforeThrow;
    private Transform _player;
    private Vector3 _offset; // Vecteur de d calage initial entre le joueur et la boule
    private Rigidbody _rb;
    public List<Vector3> _contactPoints;
    private Vector3 _target;
    private int _destPoint;
    private bool _isReturning = false;
    private float _distance;
    private bool _canThrow = true;
    private bool _resetedBoul = false;


    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = FindAnyObjectByType<PlayerStateMachine>()?.transform;
    }

    void Start()
    {
        //_player = GetComponentsInParent<Transform>()[1];
        _contactPoints = new List<Vector3>();
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _player.position.z);
        _offset = (_player.position - this.transform.position) * _size;// Calcule le vecteur de d calage initial entre le joueur et la boule
        _distance = Vector3.Distance(_player.position, this.transform.position);
    }

    private void Update()
    {
        if(_player == null) 
            _player = FindAnyObjectByType<PlayerStateMachine>()?.transform;

        if (Input.GetKeyDown(KeyCode.LeftShift) && _canThrow)
        {
            this.GetComponentInChildren<SphereCollider>().material = _bounce;
            _isThrowing = true;
            updateThrowing();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && _isThrowing)
        {
            setUpBoule();

        }
        if(!(Mathf.Abs(_distance - Vector3.Distance(_player.position, this.transform.position)) > 1.0f) && !(Mathf.Abs(_distance - Vector3.Distance(_player.position, this.transform.position)) < -1.0f) && _resetedBoul)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _resetedBoul = false;
            print("reseted");
        }

    }

   
    private void FixedUpdate()
    {
        if(!_isThrowing)
            updateRotationBoule();
        if (_isReturning)
            returnBoule();

    }

    private void updateThrowing()
    {
        //this.transform.SetParent(null);
        if (_beforeThrow == null)
            _beforeThrow = this.transform;

        _rb.AddForce(-this.transform.forward * Time.deltaTime * _speedThrowing, ForceMode.VelocityChange);

    }

    private void returnBoule()
    {
        

        Vector3 dir = (_target - this.transform.position).normalized;
        transform.Translate(dir * Time.deltaTime * _speedBack, Space.World);

        if (_target == _contactPoints[0] && Vector3.Distance(transform.position, _target) < 0.1f)
        {
            _contactPoints.Clear();
            this.transform.rotation = _beforeThrow.rotation;
            _beforeThrow = null;
            _isReturning = false;
            _isThrowing = false;
            //this.transform.SetParent(_player);
            resetBool();
            return;

        }
        if (Vector3.Distance(transform.position, _target) < 0.1f)
        {
            _destPoint--;
            _target = _contactPoints[_destPoint];
        }
        

    }
    private void updateRotationBoule()
    {
        if (_resetedBoul)
            return;

        transform.RotateAround(_player.transform.position, (_clockwise ? Vector3.forward : -Vector3.forward) * 2, _rotationSpeed * Time.deltaTime);
        transform.LookAt(_player);

       /* Quaternion rotation = Quaternion.Euler(0, 0, Time.deltaTime * (_clockwise ? _rotationSpeed : -_rotationSpeed));// Calcule la nouvelle position de la boule autour du joueur
        _offset = _offset.normalized * _offset.magnitude; // Normalise l'_offset pour maintenir une distance constante
        _offset = rotation * _offset;
        Vector3 newPosition = _player.position + _offset;
        transform.position = newPosition;// D place la boule   la nouvelle position*/
        // Assurez-vous que la boule regarde toujours vers le joueur

    }
    private void resetBool()
    {
        _resetedBoul = true;

        if (Mathf.Abs(_distance - Vector3.Distance(_player.position, this.transform.position)) > 1.0f )
        {
            _rb.AddForce(this.transform.forward * Time.deltaTime * 0.5f, ForceMode.VelocityChange);
            print("plus grand");
            // Téléporte la boule à la nouvelle position
        }
        else if (Mathf.Abs(_distance - Vector3.Distance(_player.position, this.transform.position)) < -1.0f)
        {
            _rb.AddForce(-this.transform.forward * Time.deltaTime * 0.5f, ForceMode.VelocityChange);
           print("plus petit");
            // Téléporte la boule à la nouvelle position
        }

    }
    private void setUpBoule()
    {
        if (_contactPoints.Count == 0)
            _contactPoints.Add(_beforeThrow.position);

        _destPoint = _contactPoints.Count;
        _target = _contactPoints[_destPoint - 1];
        _isReturning = true;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        this.GetComponentInChildren<SphereCollider>().material = null;
    }
  
    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.tag == "rebondi")
        _clockwise = !_clockwise; // Change le sens de rotation lorsque la collision se produit

        _canThrow = false;

        if (_isThrowing)
            _contactPoints.Add(this.transform.position);

        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.SetActive(false);
            if(_isThrowing)
                setUpBoule();

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        print("_distance = " + _distance + " distance réel : " + Vector3.Distance(_player.position, this.transform.position));

        resetBool();
        _canThrow = true;
    }


}