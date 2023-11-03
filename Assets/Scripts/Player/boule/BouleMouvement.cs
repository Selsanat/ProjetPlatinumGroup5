using System.Collections.Generic;
using UnityEngine;


public class BouleMouvement : MonoBehaviour
{
    #region public variables
    [Header("Le material de la boule ne pas touch�")]
    [Space(5)]
    public PhysicMaterial _bounce;
    [Space(5)]

    [Header("Le player ne pas touch�")]
    public Transform _playerPivot;
    [Header("Le player ne pas touch�")]
    public Transform _playerTransform;
    public bool _clockwise = true;

    #endregion

    #region Private variables

    [HideInInspector]
    public PlayerInput _playerInputs;
    [HideInInspector]
    public PlayerStateMachine ParentMachine;
    private Vector3 _beforeThrow;
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
    private float _distancePoints = 0.5f;
    private RoundManager _roundManager => RoundManager.Instance;
    private ManagerManager _manager => ManagerManager.Instance;
    public BouleParams _bouleParams;// => _manager.bouleParams;

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
        GUILayout.Label("distance base : " + _distance);
        GUILayout.Label("state idle: " + stateBoule);

        GUILayout.Label("timer : " + _timeThrowing);
        GUILayout.Label("distance : " + Vector3.Distance(_playerPivot.position, this.transform.position));
    }
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 vec = new Vector3(this.transform.position.x, this.transform.position.y, _playerPivot.position.z);
        Gizmos.DrawWireSphere(_playerPivot.position, Vector3.Distance(_playerPivot.position, vec));
    }
#endif
    private void Awake()
    {
        _collidingObject = new List<GameObject>();
        _rb = GetComponent<Rigidbody>();
        _sphereCollider = GetComponentInChildren<SphereCollider>();
        ParentMachine = GetComponentInParent<PlayerStateMachine>();

    }

    void Start()
    {
        _clockwise = _bouleParams._clockwise;
        _contactPoints = new List<Vector3>();
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _playerPivot.position.z);
        _distance = Vector3.Distance(_playerPivot.position, this.transform.position);
    }

    private void Update()
    {
        if (_playerInputs.triggers>0 && stateBoule == StateBoule.idle && ParentMachine._iMouvementLockedReader.isMouvementLocked == false) // Quand le joueur appuie sur la touche
        {
            _sphereCollider.material = _bounce;
            stateBoule = StateBoule.throwing;
            updateThrowing();
        }
        if (_playerInputs.triggers<1 && stateBoule == StateBoule.throwing)
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
            if(_timeThrowing >= _bouleParams._retourTimeMax)
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

        

        if ((Mathf.Abs(_distance - Vector3.Distance(_playerPivot.position, this.transform.position)) <= 0.25f))
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
        else if ((Mathf.Abs(_distance - Vector3.Distance(_playerPivot.position, this.transform.position)) > 0.5f) && (stateBoule == StateBoule.idle || stateBoule == StateBoule.reseting))
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

        _rb.AddForce(-this.transform.forward * Time.fixedDeltaTime * _bouleParams._speedThrowing, ForceMode.VelocityChange);

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
                float pourcentageComplete = _lerpTime / _bouleParams._lerpDurationSlow;

                currentSpeed = Mathf.Lerp(_bouleParams._speedBack, 0, _bouleParams._lerpCurve.Evaluate(pourcentageComplete));
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
                float pourcentageComplete = _lerpTime / _bouleParams._lerpDurationFast;
                currentSpeed = Mathf.Lerp(0.2f, _bouleParams._speedBack, _bouleParams._lerpCurve.Evaluate(pourcentageComplete));
                transform.Translate(dir * Time.fixedDeltaTime * currentSpeed, Space.World);
            }

        }
        else 
        {
            transform.Translate(dir * Time.fixedDeltaTime * _bouleParams._speedBack, Space.World);
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

        transform.RotateAround(_playerPivot.transform.position, (_clockwise ? Vector3.forward : -Vector3.forward) * 2, _bouleParams._rotationSpeed * Time.fixedDeltaTime);
        transform.LookAt(_playerPivot);
    }


    private void resetBool() // Quand la boule est trop loin ou trop proche du joueur
    {

        if (Mathf.Abs(_distance - Vector3.Distance(_playerPivot.position, this.transform.position)) > 0.5f)
        {
            stateBoule = StateBoule.reseting;
            transform.LookAt(_playerPivot);
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            if (_distance <= Vector3.Distance(_playerPivot.position, this.transform.position))
            {
                _rb.AddForce(this.transform.forward * Time.deltaTime * _bouleParams._resetSpeed, ForceMode.VelocityChange);
            }
            else if (_distance > Vector3.Distance(_playerPivot.position, this.transform.position))
            {
                _rb.AddForce(-this.transform.forward * Time.deltaTime * _bouleParams._resetSpeed, ForceMode.VelocityChange);
            }

            // T�l�porte la boule � la nouvelle position
        }
        else
        {
            stateBoule = StateBoule.idle;
            _sphereCollider.isTrigger = false;
            this.transform.SetParent(_playerPivot);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _playerPivot.position.z);


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
            if(collision.gameObject != ParentMachine.gameObject)
            {
                PlayerStateMachine StateMachine = collision.gameObject.GetComponentInChildren<PlayerStateMachine>();
                if (StateMachine.CurrentState != StateMachine.deathState)
                {
                    RoundManager.Instance.KillPlayer(StateMachine);
                    StateMachine.ChangeState(StateMachine.deathState);
                }
                if (stateBoule == StateBoule.throwing)
                    setUpBoule();
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