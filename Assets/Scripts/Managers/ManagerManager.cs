using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;

public class ManagerManager : MonoBehaviour
{
    public static ManagerManager Instance;
    public GameParams gameParams;
    public Canvas selectionPersoCanvas;
    public Dictionary<InputDevice, RoundManager.Team> Players = new Dictionary<InputDevice, RoundManager.Team>();
    public List<CharacterSelector> characterSelector = new List<CharacterSelector>() ;
    public Button StartGame;
    public Toggle ReadyToFight;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
        DontDestroyOnLoad(gameObject);
    }


}
