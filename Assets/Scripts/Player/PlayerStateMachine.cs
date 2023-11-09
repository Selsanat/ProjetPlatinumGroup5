using DetectCollisionExtension;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Movements")]
    public MovementParams movementsParam;
    public IMouvementLockedReader _iMouvementLockedReader => GetComponent<IMouvementLockedReader>();
    public  IMouvementLockedWriter _iMouvementLockedWriter => GetComponent<IMouvementLockedWriter>();
    private PlayerInput _playerInputs;

    public IdleState stateIdle { get; } = new IdleState();
    public WalkState stateWalk { get; } = new WalkState();
    public AccelerateState stateAccelerate { get; } = new AccelerateState();
    public DecelerateState stateDecelerate { get; } = new DecelerateState();
    public TurnAccelerateState stateTurnAccelerateState { get; } = new TurnAccelerateState();
    public TurnDecelerateState turnDecelerateState { get; } = new TurnDecelerateState();

    public FallState fallState { get; } = new FallState();
    public JumpState jumpState { get; } = new JumpState();
    public DeathState deathState { get; } = new DeathState();

    public TemplateState[] AllStates => new TemplateState[]
    {
        stateIdle,
        stateWalk,
        stateAccelerate,
        stateDecelerate,
        stateTurnAccelerateState,
        turnDecelerateState,
        jumpState,
        fallState,
        deathState
    };

    public TemplateState StartState => stateIdle;
    public TemplateState CurrentState { get; private set; }
    public TemplateState PreviousState { get; private set; }

    public Vector2 velocity;
    public float JumpBuffer;
    public bool activeHUD = false;

    public BouleMouvement bouleMouvement;
    private void Awake()
    {
        bouleMouvement = GetComponentInChildren<BouleMouvement>();
        DontDestroyOnLoad(gameObject);

        _InitAllStates();
        _playerInputs = InputsManager.Instance.AddComponent<PlayerInput>();
        _playerInputs._ipaPlayercontrols = this.GetComponent<UnityEngine.InputSystem.PlayerInput>().actions;
        _playerInputs.IOrient = this.GetComponent<IOrientWriter>();
        _playerInputs.jump = this.GetComponent<IWantsJumpWriter>();
        if(GetComponentInChildren<BouleMouvement>()!= null)
        GetComponentInChildren<BouleMouvement>()._playerInputs = _playerInputs;

        InputsManager.PlayersInputs inputs = new InputsManager.PlayersInputs(_playerInputs, this);
        InputsManager.Instance.playerInputs.Add(inputs);


        if (ManagerManager.Instance.Players.Count > 0)
        {
            var device = _playerInputs._ipaPlayercontrols.devices.Value;
            RoundManager.Player player = new RoundManager.Player(inputs, ManagerManager.Instance.Players[device[0]]);
            RoundManager.Instance.players.Add(player);
            RoundManager.Instance.alivePlayers.Add(player);
        }
    }
    void Start()
    {
        ChangeState(StartState);
    }

    private void FixedUpdate()
    {
        Vector3 x = gameObject.GetComponentInChildren<Animator>().gameObject.transform.localScale;
        gameObject.GetComponentInChildren<Animator>().gameObject.transform.localScale = new Vector3(Mathf.Sign(velocity.x), x.y,x.z);
        CurrentState.StateUpdate();

    }
    private void OnGUI()
    {
        if (!activeHUD) return;
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("State machine :" + CurrentState);
        GUILayout.Label(DetectCollision.isColliding(Mathf.Sign(velocity.x) * Vector2.right,transform, Vector2.zero) ? "OnGround" : "InAir");
        GUILayout.Label(velocity+"");
        GUILayout.Label(Time.time + "");
        GUILayout.Label("Jump Buffer :" + JumpBuffer);
        GUILayout.EndVertical();
    }
    private void _InitAllStates()
    {
        foreach (var state in AllStates)
        {
            state.Init(this);
        }
    }
    public void ChangeState(TemplateState state)
    {
        if (CurrentState != null)
        {
            CurrentState.StateExit(state);
        }
        //print("State was :" + CurrentState + " And now is : " + state);
        PreviousState = CurrentState;
        CurrentState = state;
        if (CurrentState != null)
        {
            CurrentState.StateEnter(state);
        }
    }

    /*public void getHit()
    {
        if(CurrentState != deathState)
        {
            ChangeState(deathState);
            
        }
    }*/
    
}
