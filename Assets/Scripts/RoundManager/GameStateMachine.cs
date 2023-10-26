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

    [System.Serializable]
    public class _choiceStates
    {
        public string[] choices = new string[0];

       public  _choiceStates(int taille)
        {
            this.choices = new string[taille];
        }
    }

    [SerializeField] public _choiceStates[] _choiceState;
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
        var info = new DirectoryInfo("Assets/Scripts/RoundManager/States");
        var fileInfo = info.GetFiles();
        foreach (GameStateTemplate State in AllStates)
        {
            foreach (FileInfo file in fileInfo)
            {
                if (State.GetType() == file.GetType())
                {
                    GameStateTemplate thatState = AllStates[AllStates.ToList().IndexOf(State)];
                    thatState.ui = Menus[AllStates.ToList().IndexOf(State)].menuObject;
                }
            }
        }
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
        print("StateChangé");
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
    public void HideAllMenusExceptThis(GameObject ui)
    {
        foreach (Menu menu in Menus)
        {
            if (menu.menuObject != ui)
            {
                menu.menuObject.SetActive(false);
            }
        }
    }
}
