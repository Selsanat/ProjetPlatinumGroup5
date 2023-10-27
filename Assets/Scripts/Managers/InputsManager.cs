using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputsManager : MonoBehaviour
{
    // Start is called before the first frame update

    //Input Manager Singleton

    public static InputsManager Instance;
    public PlayerInputManager playerInputManager => gameObject.GetComponent<PlayerInputManager>();
    public List<PlayerStateMachine> PlayersStateMachines = new List<PlayerStateMachine>();
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
    }
}
