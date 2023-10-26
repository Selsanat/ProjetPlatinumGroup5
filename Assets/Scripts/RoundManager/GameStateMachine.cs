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
    public StateParam paramState { get; } = new StateParam();

    public StateSelectionPerso selectionPersoState { get; } = new StateSelectionPerso();

    public GameStateTemplate[] AllStates => new GameStateTemplate[]
    {
        menuState,
        paramState,
        selectionPersoState
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
                if (State.GetType().ToString() == file.Name.Replace(".cs", string.Empty))
                {
                    GameStateTemplate thatState = AllStates[AllStates.ToList().IndexOf(State)];
                    thatState.ui = Menus[AllStates.ToList().IndexOf(State)].menuObject;
                }
            }
        }

        foreach (GameStateTemplate State in AllStates)
        {
            print("Scene : " + State.GetType());
            print("ui : " + State.ui);
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
        print(state);
        if (CurrentState != null)
        {
            CurrentState.StateEnter(state);
        }
    }
    
    public void ChangeState(int state)
    {
        var info = new DirectoryInfo("Assets/Scripts/RoundManager/States");
        var fileInfo = info.GetFiles();
        foreach (GameStateTemplate State in AllStates)
        {
            if (State.GetType().ToString() == fileInfo[state*2].Name.Replace(".cs", string.Empty))
            {
                GameStateTemplate thatState = AllStates[AllStates.ToList().IndexOf(State)];
                ChangeState(thatState);
            }
        }
    }
    public void HideAllMenusExceptThis(GameObject ui)
    {
        foreach (Menu menu in Menus)
        {
                menu.menuObject.SetActive(false);
        }
        ui.SetActive(true);
        print(ui.GetComponentInChildren < Button>());
        ui.GetComponentInChildren<Button>().Select();
    }
}
