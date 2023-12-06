using System.Collections;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameStateMachine : MonoBehaviour
{
    public bool activeHUD;
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
    public StateMapSelection MapSelectionState { get; } = new StateMapSelection();

    public GameStateTemplate[] AllStates => new GameStateTemplate[]
    {
        menuState,
        paramState,
        selectionPersoState,
        roundState,
        endRound,
        MapSelectionState
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
        SoundManager.instance.PlayClip("Drill");
        ChangeState(StartState);
    }

    private void FixedUpdate()
    {
        if(CurrentState == null) CurrentState = menuState;
        CurrentState.StateUpdate();
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
    
    public void ChangeState(string state)
    {
        if(state == CurrentState.GetType().ToString()) return;
        StartCoroutine(ChangeStateCoroutine(state));
    }
    private IEnumerator ChangeStateCoroutine(string state)
    {
        if (asyncLoadLevel != null)
        {
            yield return new WaitUntil(() => asyncLoadLevel.isDone);

        }

        GameStateTemplate thatState = _GetStateByName(state);
        ChangeState(thatState);
    }

    public void ChangeScene(string scene)
    {
        if(scene == SceneManager.GetActiveScene().name) return;
        CameraTransition.Instance.FreezeIt();
        EventSystem.current.SetSelectedGameObject(null);
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
            /*if (ui.GetComponentInChildren<Slider>() != null)
                ui.GetComponentInChildren<Slider>().Select();*/
            if (ui.GetComponentInChildren<Button>() != null)
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

    private GameStateTemplate _GetStateByName(string name)
    {
        foreach(GameStateTemplate state in AllStates)
        {
            if (state.GetType().Name == name) return state;
        }

        return null;
    }


}
