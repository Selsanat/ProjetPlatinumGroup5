using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class GameStateMachine : MonoBehaviour
{
    [System.Serializable]
    public class Menu
    {
        public GameObject menuObject;
        [HideInInspector]
        public string thisMenu;
        [HideInInspector]
        public Button[] buttons;
    }
    [SerializeField]
    public string[][] _choiceState;
    [SerializeField]
    public string[] _choices;
    [SerializeField]
    public Menu[] Menus;
    public MenuState menuState { get; } = new MenuState();

    public GameStateTemplate[] AllStates => new GameStateTemplate[]
    {
        menuState,
    };
    public GameStateTemplate StartState => menuState;
    public GameStateTemplate CurrentState { get; private set; }
    public GameStateTemplate PreviousState { get; private set; }

    private void Awake()
    {
        _InitAllStates();
    }
    void Start()
    {
        
        ChangeState(StartState);
    }

    private void FixedUpdate()
    {
        CurrentState.StateUpdate();

    }
    private void OnGUI()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("Menu State :");
        GUILayout.TextField("" + CurrentState);
        GUILayout.EndVertical();
    }
    private void _InitAllStates()
    {
        foreach (var state in AllStates)
        {
            state.Init(this);
        }
    }
    public void ChangeState(GameStateTemplate state)
    {
        if (CurrentState != null)
        {
            CurrentState.StateExit(state);
        }
        PreviousState = CurrentState;
        CurrentState = state;
        if (CurrentState != null)
        {
            state.ui = Menus[0].menuObject;
            CurrentState.StateEnter(state);
        }
    }
    public void ChangeState(int state, string menu)
    {
        var info = new DirectoryInfo("Assets/Scripts/RoundManager/States");
        var fileInfo = info.GetFiles();
        foreach (GameStateTemplate State in AllStates)
        {
            
            if (State.GetType() == fileInfo[state].GetType())
            {
                GameStateTemplate thatState = AllStates[AllStates.ToList().IndexOf(State)];
                thatState.ui = Menus.ToList().Find(x => x.thisMenu == menu).menuObject;
                print(thatState.ui);
                ChangeState(thatState);
            }
        }
    }
}
