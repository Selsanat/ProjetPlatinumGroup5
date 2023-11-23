using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Toggle = UnityEngine.UI.Toggle;

public class ManagerManager : MonoBehaviour
{
    public static ManagerManager Instance;
    public GameParams gameParams;
    public BouleParams bouleParams;
    public Canvas selectionPersoCanvas;
    public Dictionary<InputDevice, RoundManager.Team> Players = new Dictionary<InputDevice, RoundManager.Team>();
    public List<CharacterSelector> characterSelector = new List<CharacterSelector>() ;
    public Button StartGame;
    public Toggle ReadyToFight;
    public HorizontalLayoutGroup horizontalLayoutGroup;
    public Volume Volume => FindObjectOfType<Volume>();
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
