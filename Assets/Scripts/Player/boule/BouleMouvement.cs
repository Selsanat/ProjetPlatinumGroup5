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
    [Header("Sens de rotation initial (true va de gauche � droite)")]
    [Space(5)]
    public bool _clockwise = true; // Sens de rotation initial
    [Space(10)]
    [Header("vitesse � laquelle la boule va revenir en place apr�s un bug de d�placement")]
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
    [Header("Combien de temps avant que la boule revienne (en seconde)")]
    [Space(5)]
    public float _retourTimeMax = 2.0f;
    [Space(10)]
    [Header("la courbe de vitesse que va prendre la boule (en terme de vitesse)")]
    [Space(5)]
    public AnimationCurve _lerpCurve;
    [Space(10)]
    [Header("Le material de la boule ne pas touch�")]
    [Space(5)]
    public PhysicMaterial _bounce;
    [Space(5)]

    [Header("Le player ne pas touch�")]
    public Transform _playerPivot;
    [Header("Le player ne pas touch�")]
    public Transform _playerTransform;

    #endregion

    #region Private variables

    private bool _isThrowing = false;
    private Vector3 _beforeThrow;
    private Vector3 _offset; // Vecteur de d calage initial entre le joueur et la boule
    private Rigidbody _rb;
    private List<Vector3> _contactPoints;
    private Vector3 _target;
    private int _destPoint;
    private float _distance;
    private SphereCollider _sphereCollider;
    private float currentSpeed = 0;
    private float _lerpTime = 0;
    private bool _isLerpSlowFinished = false;
    private List<GameObject> _collidingObject;
    private Vector3 _vecHit;
    private float _timeThrowing = 0;
    private float _distancePoints = 0.25f;
    private RoundManager _roundManager => RoundManager.Instance;

    private enum StateBoule
    {
        idle,
        throwing,
        returning,
        reseting,
        death
    }
    StateBoule stateBoule = StateBoule.idle;
    private void OnGUI()
    {
        GUILayout.Label("state idle: " + stateBoule);
        GUILayout.Label("distance base : " + _distance);
        GUILayout.Label("timer : " + _timeThrowing);
        GUILayout.Label("distance : " + Vector3.Distance(_playerPivot.position, this.transform.position));
    }
    #endregion

    private void Awake()
    {
        _collidingObject = new List<GameObject>();
        _rb = GetComponent<Rigidbody>();
        _sphereCollider = GetComponentInChildren<SphereCollider>();
    }

    void Start()
    {
        _contactPoints = new List<Vector3>();
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _playerPivot.position.z);
        _offset = (_playerPivot.position - this.transform.position) * _size;// Calcule le vecteur de d calage initial entre le joueur et la boule
        _distance = Vector3.Distance(_playerPivot.position, this.transform.position);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && stateBoule == StateBoule.idle) // Quand le joueur appuie sur la touche
        {
            _sphereCollider.material = _bounce;
            stateBoule = StateBoule.throwing;
            updateThrowing();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && stateBoule == StateBoule.throwing)
        {
            setUpBoule();
            _timeThrowing = 0;
        }

        stayup();
        
        if (stateBoule == StateBoule.reseting && _collidingObject.Count != 0)
        {
            _clockwise = !_clockwise;
        }
        if(stateBoule == StateBoule.throwing)
        {
            _timeThrowing += Time.deltaTime;
            if(_timeThrowing >= _retourTimeMax)
            {
                setUpBoule();
                _timeThrowing = 0;
            }
        }

    }


    private void FixedUpdate()
    {
        if (stateBoule == StateBoule.idle)
            updateRotationBoule();
        if (stateBoule == StateBoule.returning)
            returnBoule();

        

        if ((Mathf.Abs(_distance - Vector3.Distance(_playerPivot.position, this.transform.position)) <= 0.1f))
        {
            
            if (stateBoule == StateBoule.reseting)
            {
                stateBoule = StateBoule.idle;
                _sphereCollider.isTrigger = false;
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
                this.transform.SetParent(_playerPivot);

            }
            else if(stateBoule == StateBoule.death)
            {
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
                this.transform.SetParent(_playerPivot);
                //anime mort
            }

        }
        else if ((Mathf.Abs(_distance - Vector3.Distance(_playerPivot.position, this.transform.position)) > 0.1f) && (stateBoule == StateBoule.idle || stateBoule == StateBoule.reseting))
        {
            resetBool();
        }


    }
    public void returnBouleDeath()
    {
        _sphereCollider.isTrigger = true;
        resetBool();
        stateBoule = StateBoule.death;
    }
    private void updateThrowing() //lanc� de la boule
    {
        if (_beforeThrow == Vector3.zero)
        {
            _beforeThrow = this.transform.position;
            _contactPoints.Add(_beforeThrow);
        }
        this.transform.SetParent(null);

        _rb.AddForce(-this.transform.forward * Time.fixedDeltaTime * _speedThrowing, ForceMode.VelocityChange);

    }

    private void returnBoule() // retour de la boule
    {
        Vector3 dir = (_target - this.transform.position).normalized;
        if (_target == _contactPoints[_contactPoints.Count - 1]) //si on est sur le premier point
        {
            if (!_isLerpSlowFinished) //lorsque l'on ralentie
            {
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
                _lerpTime += Time.fixedDeltaTime;
                float pourcentageComplete = _lerpTime / _lerpDurationSlow;

                currentSpeed = Mathf.Lerp(_speedBack, 0, _lerpCurve.Evaluate(pourcentageComplete));
                transform.Translate(-dir * Time.fixedDeltaTime * currentSpeed / 2.4f, Space.World);
                if (currentSpeed == 0)
                {
                    _isLerpSlowFinished = true;
                    _lerpTime = 0;
                    _rb.velocity = Vector3.zero;
                    _rb.angularVelocity = Vector3.zero;
                }

            }
            else if (_isLerpSlowFinished) //lorsque l'on acc�l�re
            {
                _lerpTime += Time.fixedDeltaTime;
                float pourcentageComplete = _lerpTime / _lerpDurationFast;
                currentSpeed = Mathf.Lerp(0.2f, _speedBack, _lerpCurve.Evaluate(pourcentageComplete));
                transform.Translate(dir * Time.fixedDeltaTime * currentSpeed, Space.World);
            }

        }
        else 
        {
            transform.Translate(dir * Time.fixedDeltaTime * _speedBack, Space.World);
        }
        if (Vector3.Distance(transform.position, _target) < _distancePoints && _target != _contactPoints[0])
        {
            print("next point");
            _destPoint--;
            _target = _contactPoints[_destPoint];
        }

        if (_target == _contactPoints[0]) //si on est sur le dernier
        {

            if (Vector3.Distance(transform.position, _target) < _distancePoints) // si on est assez proche du dernier point
            {
                _contactPoints.Clear();

                stateBoule = StateBoule.idle;
                _beforeThrow = Vector3.zero;
                if (Vector3.Distance(transform.position, _playerPivot.position) > _distancePoints) // si on est loin du joueur, alors on le fait revenir et l empechant de collide avec autres chose
                {
                    _sphereCollider.isTrigger = true;
                    resetBool();

                }

                print("last");
                return;
            }


        }
        


    }
    private void updateRotationBoule() // rotation de la boule
    {
        if (stateBoule == StateBoule.reseting)
            return;

        transform.RotateAround(_playerPivot.transform.position, (_clockwise ? Vector3.forward : -Vector3.forward) * 2, _rotationSpeed * Time.fixedDeltaTime);
        transform.LookAt(_playerPivot);
    }


    private void resetBool() // Quand la boule est trop loin ou trop proche du joueur
    {

        if (Mathf.Abs(_distance - Vector3.Distance(_playerPivot.position, this.transform.position)) > 0.1f)
        {
            stateBoule = StateBoule.reseting;
            transform.LookAt(_playerPivot);
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            if (_distance <= Vector3.Distance(_playerPivot.position, this.transform.position))
            {
                _rb.AddForce(this.transform.forward * Time.deltaTime * _resetSpeed, ForceMode.VelocityChange);
            }
            else if (_distance > Vector3.Distance(_playerPivot.position, this.transform.position))
            {
                _rb.AddForce(-this.transform.forward * Time.deltaTime * _resetSpeed, ForceMode.VelocityChange);
            }

            // T�l�porte la boule � la nouvelle position
        }
        else
        {
            stateBoule = StateBoule.idle;
            _sphereCollider.isTrigger = false;
            this.transform.SetParent(_playerPivot);


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
        currentSpeed = 0;
        _isLerpSlowFinished = false;
        _lerpTime = 0;
        stateBoule = StateBoule.returning;

    }
    private void stayup()
    {
        RaycastHit hit;
        if (_collidingObject.Count != 0 && Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f) && stateBoule == StateBoule.idle)
        {
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);

            if ((this.transform.position.y < _vecHit.y))
            {
                transform.position = new Vector3(this.transform.position.x, _vecHit.y, this.transform.position.z);
            }

        }
        else if(_collidingObject.Count != 0 && Physics.Raycast(transform.position, Vector3.up, out hit, 0.5f) && stateBoule == StateBoule.idle)
        {

            if ((this.transform.position.y > _vecHit.y))
            {
                Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);

                transform.position = new Vector3(this.transform.position.x, _vecHit.y, this.transform.position.z);

            }

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == _playerTransform.gameObject)
        {
            return;
        }
        //if(collision.gameObject.tag == "rebondi")
        
        if(collision.gameObject.tag == "Player" )
        {
            if(collision.gameObject != transform.parent.gameObject)
            {
                RoundManager.Instance.KillPlayer(collision.gameObject.GetComponent<PlayerStateMachine>());
                collision.gameObject.GetComponentInChildren<PlayerStateMachine>()._iMouvementLockedWriter.isMouvementLocked = true;
                print("player");
                //collision.gameObject.GetComponentInChildren<PlayerStateMachine>().ChangeState(GetComponentInChildren<PlayerStateMachine>().deathState);
                //_roundManager.playerDied(collision.gameObject.GetComponentInChildren<playerClass>());
                if (stateBoule == StateBoule.throwing)
                    setUpBoule();
                //Destroy(collision.gameObject);

            }
            else
                return;
        }
        _clockwise = !_clockwise; // Change le sens de rotation lorsque la collision se produit
        _collidingObject.Add(collision.gameObject);
        if (stateBoule == StateBoule.throwing)
            _contactPoints.Add(this.transform.position);
        _vecHit = collision.contacts[0].point;
    }

    
    private void OnCollisionExit(Collision collision)
    {
        _collidingObject.Remove(collision.gameObject);
        if (stateBoule == StateBoule.idle && _collidingObject.Count == 0)
            resetBool();
        if (_collidingObject.Count > 0)
            _clockwise = !_clockwise;
    }


}