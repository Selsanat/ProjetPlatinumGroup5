using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class GameStateMachine : MonoBehaviour
{
    [System.Serializable]
    public class Menu
    {
        public GameObject menuObject;
        public Button[] buttons;
    }

    public enum GameState
    {
        menuState,
        playState
    }
    public Menu[] Menus;
    public MenuState menuState { get; } = new MenuState();

    public GameStateTemplate[] AllStates => new GameStateTemplate[]
    {
        menuState
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
            CurrentState.StateEnter(state);
        }
    }
}
