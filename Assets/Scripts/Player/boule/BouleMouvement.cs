using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using Unity.VisualScripting;

public class BouleMouvement : MonoBehaviour
{
    #region public variables

    [Header("Vitesse de rotation de la boule autour du joueur")]
    [Space(5)]
    public float _rotationSpeed = 100.0f; // Vitesse de rotation de la boule autour du joueur
    [Space(10)]
    [Header("Sens de rotation initial (true va de gauche à droite)")]
    [Space(5)]
    public bool _clockwise = true; // Sens de rotation initial
    [Space(10)]
    [Header("vitesse à laquelle la boule va revenir en place après un bug de déplacement")]
    [Space(5)]
    public float _resetSpeed = 400;
    [Space(10)]
    [Header("Distance entre la boule et le player (c'est une multiplication donc ca va vite)")]
    [Space(5)]
    public float _size = 1.0f;
    [Space(10)]
    [Header("Vitesse de lancer de la boule")]
    [Space(5)]
    public float _speedThrowing = 10.0f;
    [Space(10)]
    [Header("Vitesse du lancer de la boule")]
    public float _speedBack = 2.0f;
    [Space(10)]
    [Header("Combien de temps avant que la boule ai sa vitesse max au retour (en seconde)")]
    [Space(5)]
    public float _lerpDurationFast = 2.0f;
    [Space(10)]
    [Header("Combien de temps avant que la boule ai fini de ralentire (en seconde)")]
    [Space(5)]
    public float _lerpDurationSlow = 2.0f;
    [Space(10)]
    [Header("la courbe de vitesse que va prendre la boule (en terme de vitesse)")]
    [Space(5)]
    public AnimationCurve _lerpCurve;
    [Space(10)]
    [Header("Le material de la boule ne pas touché")]
    [Space(5)]
    public PhysicMaterial _bounce;
    #endregion

    #region Private variables

    private bool _isThrowing = false;
    private Vector3 _beforeThrow;
    private Transform _player;
    private Vector3 _offset; // Vecteur de d calage initial entre le joueur et la boule
    private Rigidbody _rb;
    private List<Vector3> _contactPoints;
    private Vector3 _target;
    private int _destPoint;
    private bool _isReturning = false;
    private float _distance;
    private bool _canThrow = true;
    private bool _resetedBoul = false;
    private SphereCollider _sphereCollider;
    private float currentSpeed = 0;
    private float _lerpTime = 0;
    private bool _isLerpSlowFinished = false;


    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = FindAnyObjectByType<PlayerStateMachine>()?.transform;
        _sphereCollider = GetComponentInChildren<SphereCollider>();
    }

    void Start()
    {
        _contactPoints = new List<Vector3>();
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _player.position.z);
        _offset = (_player.position - this.transform.position) * _size;// Calcule le vecteur de d calage initial entre le joueur et la boule
        _distance = Vector3.Distance(_player.position, this.transform.position);
    }

    private void Update()
    {
        if(_player == null) 
            _player = FindAnyObjectByType<PlayerStateMachine>()?.transform;

        if (Input.GetKeyDown(KeyCode.LeftShift) && _canThrow && !_isReturning)
        {
            _sphereCollider.material = _bounce;
            _isThrowing = true;
            updateThrowing();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && _isThrowing && !_isReturning)
        {
            setUpBoule();

        }
        if(!(Mathf.Abs(_distance - Vector3.Distance(_player.position, this.transform.position)) > 0.1f) && !(Mathf.Abs(_distance - Vector3.Distance(_player.position, this.transform.position)) < -0.1f) 
            && _resetedBoul && !_isThrowing && !_isReturning)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _resetedBoul = false;
            _sphereCollider.isTrigger = false;

            //print("reseted");
        }

    }

   
    private void FixedUpdate()
    {
        if(!_isThrowing)
            updateRotationBoule();
        if (_isReturning)
            returnBoule();

    }

    private void updateThrowing() //lancé de la boule
    {
        if (_beforeThrow == Vector3.zero)
        {
            _beforeThrow = this.transform.position;
            _contactPoints.Add(_beforeThrow);
        }
        this.transform.SetParent(null);

        _rb.AddForce(-this.transform.forward * Time.deltaTime * _speedThrowing, ForceMode.VelocityChange);

    }


    private void returnBoule() // retour de la boule
    {
        Vector3 dir = (_target - this.transform.position).normalized;
        if (_target == _contactPoints[_contactPoints.Count - 1])
        {
            /*if(!_isLerpSlowFinished)
            {
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
                _lerpTime += Time.deltaTime;
                float pourcentageComplete = _lerpTime / _lerpDurationSlow;
                currentSpeed = Mathf.Lerp(_speedThrowing, 0, _lerpCurve.Evaluate(pourcentageComplete));
                _rb.AddForce(-this.transform.forward * Time.deltaTime * currentSpeed, ForceMode.VelocityChange);
                if (currentSpeed == 0)
                {
                    _isLerpSlowFinished = true;
                    _lerpTime = 0;
                    _rb.velocity = Vector3.zero;
                    _rb.angularVelocity = Vector3.zero;
                }

            }
            else*/ //if(_isLerpSlowFinished)
            {
                _lerpTime += Time.deltaTime;
                float pourcentageComplete = _lerpTime / _lerpDurationFast;
                currentSpeed = Mathf.Lerp(0, _speedBack, _lerpCurve.Evaluate(pourcentageComplete));
                transform.Translate(dir * Time.deltaTime * currentSpeed, Space.World);
            }
            
        }
        else
        {
            
            transform.Translate(dir * Time.deltaTime * _speedBack, Space.World);

        }


        if (_target == _contactPoints[0] )
        {

            if (Vector3.Distance(transform.position, _target) < 0.1f )
            {
                _contactPoints.Clear();
                _isReturning = false;
                _isThrowing = false;
                this.transform.SetParent(_player);
                _beforeThrow = Vector3.zero;
                if(Vector3.Distance(transform.position, _player.position) > 0.1f)
                {
                    _sphereCollider.isTrigger = true;
                    resetBool();
                }

                print("last");
                return;
            }
            

        }
        if (Vector3.Distance(transform.position, _target) < 0.1f)
        {

            _destPoint--;
            _target = _contactPoints[_destPoint];
        }
        

    }
    private void updateRotationBoule() // rotation de la boule
    {
        if (_resetedBoul)
            return;

        transform.RotateAround(_player.transform.position, (_clockwise ? Vector3.forward : -Vector3.forward) * 2, _rotationSpeed * Time.deltaTime);
        transform.LookAt(_player);
    }


    private void resetBool() // Quand la boule est trop loin ou trop proche du joueur
    {
        _resetedBoul = true;
        transform.LookAt(_player);
        if (Mathf.Abs(_distance - Vector3.Distance(_player.position, this.transform.position)) > 0.1f )
        {
            if(_distance <= Vector3.Distance(_player.position, this.transform.position) )
            {
                _rb.AddForce(this.transform.forward * Time.deltaTime * _resetSpeed, ForceMode.VelocityChange);
                print("plus grand");
            }
            else if (_distance > Vector3.Distance(_player.position, this.transform.position))
            {
                _rb.AddForce(-this.transform.forward * Time.deltaTime * _resetSpeed, ForceMode.VelocityChange);
                print("plus petit");

            }

            // Téléporte la boule à la nouvelle position
        }
        

    }
    private void setUpBoule() // set up la boule Quand le joueur relache la touche
    {
        if (_contactPoints.Count == 0)
            _contactPoints.Add(_beforeThrow);

        _destPoint = _contactPoints.Count;
        _target = _contactPoints[_destPoint - 1];
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _sphereCollider.material = null;
        currentSpeed = _speedThrowing / 2;
        _isLerpSlowFinished = false;
        _lerpTime = 0;
        _isReturning = true;

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
        if(!_isThrowing && !_isReturning)
            resetBool();

        _canThrow = true;
    }


}