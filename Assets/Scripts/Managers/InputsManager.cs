using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputsManager : MonoBehaviour
{
    // Start is called before the first frame update

    //Input Manager Singleton

    public static InputsManager Instance;
    public PlayerInputManager _playerInputManager => GetComponent<PlayerInputManager>();
    public List<PlayersInputs> playerInputs = new List<PlayersInputs>();
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
}
