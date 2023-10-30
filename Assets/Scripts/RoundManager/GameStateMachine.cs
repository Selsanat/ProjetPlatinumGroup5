using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        public Object[] scenes;
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
    public StateRound roundState { get; } = new StateRound();
    public RoundEnd endRound { get; } = new RoundEnd();

    public GameStateTemplate[] AllStates => new GameStateTemplate[]
    {
        menuState,
        paramState,
        selectionPersoState,
        roundState,
        endRound
    };
    public GameStateTemplate StartState => menuState;
    public GameStateTemplate CurrentState { get; private set; }
    public GameStateTemplate PreviousState { get; private set; }

    private AsyncOperation asyncLoadLevel = null;

    public static GameStateMachine Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
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
                    if(thatState.ui != null && Menus.Length >= AllStates.ToList().IndexOf(State))
                    thatState.ui = Menus[AllStates.ToList().IndexOf(State)].menuObject;
                }
            }
        }
        ChangeState(StartState);
    }

    private void FixedUpdate()
    {
        if(CurrentState == null) CurrentState = menuState;
        CurrentState.StateUpdate();
    }
    private void OnGUI()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("Menu State :");
        GUILayout.TextField("" + CurrentState);
        foreach (Menu menu in Menus)
        {
            if (menu.thisMenu == CurrentState.ToString())
            {
                GUILayout.Label("Ma scene = ");
                GUILayout.TextField(""+ menu.menuObject);
            }
        }

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
    
    public void ChangeState(int state)
    {
        StartCoroutine(ChangeStateCoroutine(state));
    }
    private IEnumerator ChangeStateCoroutine(int state)
    {
        if (asyncLoadLevel != null)
            yield return new WaitUntil(() => asyncLoadLevel.isDone);
        var info = new DirectoryInfo("Assets/Scripts/RoundManager/States");
        var fileInfo = info.GetFiles();
        foreach (GameStateTemplate State in AllStates)
        {
            if (state * 2 < fileInfo.Length)
            {
                if (State.GetType().ToString() == fileInfo[state * 2].Name.Replace(".cs", string.Empty))
                {
                    GameStateTemplate thatState = AllStates[AllStates.ToList().IndexOf(State)];
                    ChangeState(thatState);
                }
            }
        }
    }

    public void ChangeScene(string scene)
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
    }

    public void HideAllMenusExceptThis(GameObject ui)
    {
        foreach (Menu menu in Menus)
        {
            if (menu.thisMenu == CurrentState.ToString())
            {
                foreach (GameStateTemplate State in AllStates)
                {
                    if (State.GetType().ToString() == menu.thisMenu)
                    {
                        ui = menu.menuObject;
                    }
                }
            }
            menu.menuObject.SetActive(false);
        }
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        if (ui != null)
        {
            ui.SetActive(true);
            ui.GetComponentInChildren<Button>().Select();
        }
    }
    public void HideAllMenusExceptThis()
    {
        foreach (Menu menu in Menus)
        {
            menu.menuObject.SetActive(false);
        }
    }


}
