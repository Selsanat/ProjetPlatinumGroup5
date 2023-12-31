using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using static InputsManager;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;
using Lofelt.NiceVibrations;
using Highlighters;

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
    public ParticleSystem particleSystem;
    public Material[] _trailRendererMaterials;
    private Image fleche => GetComponentsInChildren<Image>()[^1];

    #endregion

    #region Private variables
    public bool _needColor = true;
    [HideInInspector]
    public PlayerInput _playerInputs;
    [HideInInspector]
    public PlayerStateMachine ParentMachine;
    public Vector3 _beforeThrow;
    public Rigidbody _rb;
    public List<Vector3> _contactPoints;
    public Vector3 _target;
    public int _destPoint;
    public float _distance;
    public SphereCollider _sphereCollider;
    public float currentSpeed = 0;
    public float _lerpTime = 0;
    public bool _isLerpSlowFinished = false;
    public List<GameObject> _collidingObject;
    public Collider[] _hits;
    public int _nbHits;
    public Vector3 _vecHit;
    public float _timeThrowing = 0;
    public LayerMask _layer;
    //return boule
    
    public BouleParams _bouleParams;// => _manager.bouleParams;
    private float _incrementation = 1;
    public SpriteRenderer SpriteRenderer => GetComponentInChildren<SpriteRenderer>();
    public Animator Animator => SpriteRenderer.gameObject.GetComponent<Animator>();
    private enum StateBoule
    {
        idle,
        throwing,
        returning,
        reseting,
        death
    }
    StateBoule stateBoule = StateBoule.idle;
    StateBoule lastState = StateBoule.idle;
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
        setUpTrail();
        setUpParticles();
    }

    private void Update()
    {
        Debug.DrawRay(this.transform.position, _rb.velocity * 10 , Color.blue);
        Animator.SetBool("Launch", stateBoule == StateBoule.throwing|| stateBoule == StateBoule.returning);
        if (stateBoule == StateBoule.throwing)
        {
            fleche.enabled = false;
            MakeSpriteLookAtWhereYouGo(_rb.velocity);
        }
        changeState();
        if (this.transform.position.z != _playerPivot.position.z)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _playerPivot.position.z);
        }
        if (_playerInputs.triggers > 0 && stateBoule == StateBoule.idle && !ParentMachine._iMouvementLockedReader.isMouvementLocked) // Quand le joueur appuie sur la touche && hits.Length == 1
        {
            /* if (hits[0] != _sphereCollider)
            {
                return;
            }*/

            _sphereCollider.material = _bounce;
            stateBoule = StateBoule.throwing;
            updateThrowing();
        }
        if (_playerInputs.triggers < 1 && stateBoule == StateBoule.throwing)
        {
            setUpBoule();
            
            _timeThrowing = 0;
        }

       
        //stayup();

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
        onCollision();
/*        switch (stateBoule)
        {
            case StateBoule.idle:
                this.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
                break;
            case StateBoule.throwing:
                this.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                break;
            case StateBoule.returning:
                this.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
                break;
            case StateBoule.reseting:
                this.GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
                break;
            case StateBoule.death:
                break;
            default:
                break;

        }*/


    }
    private void setUpTrail()
    {
        TrailRenderer trailRenderer = this.gameObject.GetComponent<TrailRenderer>();
        trailRenderer.material = _trailRendererMaterials[this.ParentMachine.team];
    }
    private void setUpParticles()
    {
        ParticleSystem ps = this.gameObject.GetComponent<ParticleSystem>();
        ps.startColor = RoundManager.Instance.teamColors[ParentMachine.team];
    }
    private void ChangeAlpha(float alpha)
    {
        SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, alpha/100);
    }
    public void resetChangeScene()
    {
        _contactPoints.Clear();
        _collidingObject.Clear();
        stateBoule = StateBoule.reseting;
    }
    private void FixedUpdate()
    {
        switch (stateBoule)
        {
            case StateBoule.idle:
                updateRotationBoule();
                _contactPoints.Clear();
                break;
            case StateBoule.returning:
                returnBoule();
                break;

            case StateBoule.reseting:
                resetBool();
                break;
            case StateBoule.death:
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
                this.transform.SetParent(_playerPivot);
                break;
            default:
                break;

        }


        if ((Mathf.Abs(_distance - Vector3.Distance(_playerPivot.position, this.transform.position)) <= _bouleParams._distanceCloseEneaughtUpdate))
        {
            
            if (stateBoule == StateBoule.reseting)
            {
                endResetboule();

            }
            else if(stateBoule == StateBoule.death)
            {
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
                this.transform.SetParent(_playerPivot);
                //anime mort
            }

        }
        else if ((Mathf.Abs(_distance - Vector3.Distance(_playerPivot.position, this.transform.position)) > _bouleParams._distanceTooFarUpdate) && (stateBoule == StateBoule.idle))
        {
            stateBoule = StateBoule.reseting;
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
        SpriteRenderer.flipX = false;
        if (_beforeThrow == Vector3.zero)
        {
            _beforeThrow = this.transform.position;
            _contactPoints.Add(_beforeThrow);
        }
        this.transform.SetParent(null);

        _rb.AddForce(-this.transform.forward * Time.fixedDeltaTime * _bouleParams._speedThrowing, ForceMode.VelocityChange);
    }
    public void MakeSpriteLookAtWhereYouGo(Vector3 dir)
    {
        SpriteRenderer.transform.LookAt(SpriteRenderer.transform.position + dir * 10);;
        float angle = Mathf.Abs(SpriteRenderer.transform.localRotation.eulerAngles.y + SpriteRenderer.transform.localRotation.eulerAngles.x);
        SpriteRenderer.transform.localRotation = Quaternion.Euler(0, -90,angle % 270 <90 ? angle*-1:angle);
    }
    private void changeState()
    {

        if(stateBoule != StateBoule.reseting)
        {
            ChangeAlpha(100);
            TrailRenderer trailRenderer = this.gameObject.GetComponent<TrailRenderer>();
            trailRenderer.emitting = true;
        }
        if (lastState == stateBoule)
            return;
        else
        {
            switch (stateBoule)
            {
                
                case StateBoule.idle:
                    SoundManager.instance.Pauseclip("Pet Return");
                    SoundManager.instance.Pauseclip("Pet Cast");
                    break;
                case StateBoule.returning:

                    SoundManager.instance.Pauseclip("Pet Cast");
                    SoundManager.instance.PlayRandomClip("Pet Return");
                    break;
                case StateBoule.reseting:
                    SoundManager.instance.Pauseclip("Pet Cast");
                    break;
                case StateBoule.throwing:
                    Instantiate(ManagerManager.Instance.castPrefab[ParentMachine.team], ParentMachine.WandTrackTransform);
                    SoundManager.instance.PlayRandomClip("Pet Cast");
                    SoundManager.instance.Pauseclip("Pet Return");
                    if(ParentMachine.GetComponent<UnityEngine.InputSystem.PlayerInput>().devices[0] is Gamepad)
                        HapticsManager.Instance.Vibrate("Pet Cast", (Gamepad)ParentMachine.GetComponent<UnityEngine.InputSystem.PlayerInput>().devices[0]);

                    break;
                case StateBoule.death:
                    SoundManager.instance.Pauseclip("Pet Return");
                    SoundManager.instance.Pauseclip("Pet Cast");
                    break;
                default:
                    break;

            }
            lastState = stateBoule;
        }
    }
    private void endResetboule()
    {
        stateBoule = StateBoule.idle;
        _sphereCollider.isTrigger = false;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        this.transform.SetParent(_playerPivot);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, _playerPivot.position.z);
    }
    private void returnBoule() // retour de la boule
    {
        if (_target == null || _contactPoints.Count == 0)
        {
            stateBoule = StateBoule.reseting;
            return;
        }

        
        Vector3 dir = (_target - this.transform.position).normalized;
        Debug.DrawRay(transform.position, dir*10, Color.red);
        MakeSpriteLookAtWhereYouGo(dir);
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
        if (Vector3.Distance(transform.position, _target) < _bouleParams._distancePoints && _target != _contactPoints[0])
        {
            _destPoint--;
            _target = _contactPoints[_destPoint];
        }

        if (_target == _contactPoints[0]) //si on est sur le dernier
        {

            if (Vector3.Distance(transform.position, _target) < _bouleParams._distancePoints) // si on est assez proche du dernier point
            {
                _contactPoints.Clear();

                stateBoule = StateBoule.idle;
                _beforeThrow = Vector3.zero;
                if (Vector3.Distance(transform.position, _playerPivot.position) > _bouleParams._distancePoints) // si on est loin du joueur, alors on le fait revenir et l empechant de collide avec autres chose
                {
                    _sphereCollider.isTrigger = true;
                    stateBoule = StateBoule.reseting;

                }

                return;
            }


        }
        


    }
    private void onCollision()
    {

        _hits = Physics.OverlapSphere(this.transform.position, _sphereCollider.radius, _layer);
        if(_hits.Length > _nbHits)
        {
            SoundManager.instance.PlayRandomClip("bounce");

            _nbHits = _hits.Length;
            foreach(var hit in _hits)
            {
                if (hit.gameObject.layer != 0)
                {
                    break;
                }
                if (stateBoule != StateBoule.reseting)
                    particleSystem.Play();
                if (ParentMachine.GetComponent<UnityEngine.InputSystem.PlayerInput>().devices[0] is Gamepad)
                    HapticsManager.Instance.Vibrate("Pet Bounce", (Gamepad)ParentMachine.GetComponent<UnityEngine.InputSystem.PlayerInput>().devices[0]);
                if (stateBoule == StateBoule.idle) Animator.SetTrigger("Bounce");
                _clockwise = !_clockwise; // Change le sens de rotation lorsque la collision se produit
                //_collidingObject.Add(hit.gameObject);
                if (stateBoule == StateBoule.throwing)
                    _contactPoints.Add(this.transform.position);
                
            }

        }
        else if(_hits.Length < _nbHits)
        {
            _nbHits = _hits.Length;
        }
    }
    private void updateRotationBoule() // rotation de la boule
    {
        if (stateBoule == StateBoule.reseting)
            return;
        fleche.enabled = true;
        SpriteRenderer.flipX = !(_clockwise && transform.rotation.y > 0 || !_clockwise && transform.rotation.y < 0);
        SpriteRenderer.transform.localRotation = Quaternion.Euler(0, -90, -90);
        transform.RotateAround(_playerPivot.transform.position, (_clockwise ? Vector3.forward : -Vector3.forward) * 2, _bouleParams._rotationSpeed * _incrementation * Time.fixedDeltaTime);
        transform.LookAt(_playerPivot);
    }


    private void resetBool() // Quand la boule est trop loin ou trop proche du joueur
    {
        ChangeAlpha(33);
        TrailRenderer trailRenderer = this.gameObject.GetComponent<TrailRenderer>();
        trailRenderer.emitting = false;
        transform.LookAt(_playerPivot);
        _sphereCollider.isTrigger = true;
        //_sphereCollider.isTrigger = true;

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        if (_distance < Vector3.Distance(_playerPivot.position, this.transform.position))
        {
            _rb.AddForce(this.transform.forward * Time.deltaTime * _bouleParams._resetSpeed , ForceMode.VelocityChange);
        }
        MakeSpriteLookAtWhereYouGo(ParentMachine.transform.position-transform.position);
        // T�l�porte la boule � la nouvelle position

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


    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject == ParentMachine.gameObject) return;
            PlayerStateMachine pst = collision.gameObject.GetComponent<PlayerStateMachine>();

        if (pst != null) 
            if (pst.team == ParentMachine.team) 
                return;

        if(collision.gameObject != this.gameObject && collision.gameObject.layer == 3)
        {
            SoundManager.instance.PlayRandomClip("Pet Kiss");
        }
            

        if (collision.gameObject.layer == 7 && stateBoule != StateBoule.throwing)
        {
            endResetboule();
        }

        if (collision.gameObject.tag == "Player")
        {
            PlayerStateMachine StateMachine = collision.gameObject.GetComponentInChildren<PlayerStateMachine>();
            if (StateMachine.CurrentState != StateMachine.deathState)
            {
                if (StateMachine.GetComponent<UnityEngine.InputSystem.PlayerInput>().devices[0] is Gamepad)
                    HapticsManager.Instance.Vibrate("Death", (Gamepad)StateMachine.GetComponent<UnityEngine.InputSystem.PlayerInput>().devices[0]);
                if (ParentMachine.GetComponent<UnityEngine.InputSystem.PlayerInput>().devices[0] is Gamepad)
                    HapticsManager.Instance.Vibrate("Kill", (Gamepad)ParentMachine.GetComponent<UnityEngine.InputSystem.PlayerInput>().devices[0]);

                SoundManager.instance.AddPist(2);

                GameObject dieprefab = Instantiate(ManagerManager.Instance.petDiePrefab, null);
                dieprefab.transform.position = StateMachine.bouleMouvement.transform.position;
                dieprefab.GetComponent<Animator>().SetFloat("Blend", StateMachine.team);
                List<Color> highlightsColors = new List<Color>()
                {
                Color.blue,
                Color.yellow,
                Color.red,
                Color.green
                };
                    dieprefab.GetComponent<Highlighter>().Settings.OuterGlowColorFront = highlightsColors[StateMachine.team];
                Instantiate(ManagerManager.Instance.diePrefab[StateMachine.team], StateMachine.transform);
                RoundManager.Instance.KillPlayer(StateMachine);
                StateMachine.ChangeState(StateMachine.deathState);
                if (!RoundManager.Instance.ShouldEndRound())
                    SoundManager.instance.PlayRandomClip("Narrator death");
                collision.gameObject.GetComponent<PlayerStateMachine>().particleSystemDeath.Play();
            }
            if (stateBoule == StateBoule.throwing)
                setUpBoule();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7 && stateBoule != StateBoule.throwing)
        {
            endResetboule();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7 && stateBoule != StateBoule.throwing)
        {
            _rb.AddForce(-this.transform.forward * Time.deltaTime * _bouleParams._resetSpeed / 20, ForceMode.VelocityChange);
            _sphereCollider.isTrigger = true;
            _incrementation = 2;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            if(stateBoule != StateBoule.throwing)
            {
                _sphereCollider.isTrigger = false;
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
                _incrementation = 1;
            }
            else if(stateBoule == StateBoule.throwing)
            {
                _sphereCollider.isTrigger = false;
                _incrementation = 1;
            }
            

        }
    }



}