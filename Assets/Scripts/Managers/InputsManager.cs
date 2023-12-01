using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputsManager : MonoBehaviour
{
    // Start is called before the first frame update

    //Input Manager Singleton

    public static InputsManager Instance;
    public PlayerInputManager _playerInputManager => GetComponent<PlayerInputManager>();
    public List<PlayersInputs> playerInputs = new List<PlayersInputs>();
    public GameObject playerPrefab;
    public class PlayersInputs
    {
        public PlayerInput _playerInputs;
        public PlayerStateMachine _playerStateMachine;
        
        public PlayersInputs(PlayerInput playerInputs, PlayerStateMachine playerStateMachine)
        {
            _playerInputs = playerInputs;
            _playerStateMachine = playerStateMachine;
        }
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
    }

    void Start()
    {
        _playerInputManager.playerPrefab = playerPrefab;
    }

    public void Update()
    {
        print(Instance.playerInputs.Count);
    }
    public void resetPlayers()
    {
        foreach(PlayerInput pi in GetComponents<PlayerInput>())
        {
            Destroy(pi);
        }
    }
}
