using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
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