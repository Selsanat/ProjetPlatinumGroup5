using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



public class RoundManager : MonoBehaviour
{

    public static RoundManager Instance;
    public List<Player> players = new List<Player>();
    public List<Player> alivePlayers = new List<Player>();
    private GameParams _gameParams => ManagerManager.Instance.gameParams;
    private ManagerManager managerManager => ManagerManager.Instance;
    private InputsManager inputsManager => InputsManager.Instance;
    private TMP_Text[] scores;
    public GameObject[] cadrants;

    public enum Team
    {
        blue,
        yellow,
        red,    
        green
    }

    public List<Color> teamColors;
    public class Player
    {
        public PlayerInput _playerInputs;
        public PlayerStateMachine _playerStateMachine;
        public Team _team;
        public int _points = 0;

        public Player(InputsManager.PlayersInputs playerStateMachine, Team team)
        {
            RoundManager.Instance.teamColors = new List<Color>()
            {
                Color.blue,
                Color.yellow,
                Color.red,
                Color.green
            };
            _playerInputs = playerStateMachine._playerInputs;
            _playerStateMachine = playerStateMachine._playerStateMachine;
            _team = team;
            
            playerStateMachine._playerStateMachine.GetComponentInChildren<SpriteRenderer>().color = RoundManager.Instance.teamColors[(int)_team];
        }
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);

        scores = transform.GetComponentsInChildren<TMP_Text>();
        foreach (var score in scores)
        {
            score.text = "";
        }
    }

    public void StartRound()
    {

        #region GetPlayableDevices
        List<InputDevice> devices = new List<InputDevice>();
        foreach (var device in InputSystem.devices)
        {
            if (device is Gamepad || device is Keyboard)
            {
                devices.Add(device);
            }
        }
        #endregion
        #region SpawnPlayersAndLinkThemTheirControls


        GameObject[] spawnpoints = GameObject.FindGameObjectsWithTag("SpawnPoints");
        if (InputsManager.Instance._playerInputManager.playerCount ==0)
        {
            for (int i = 0; i < managerManager.Players.Count; i++)
            {
                var player = InputsManager.Instance._playerInputManager.JoinPlayer(-1, -1, null, managerManager.Players.Keys.ToList()[i]);
                player.transform.position = spawnpoints[i].transform.position;
                PlayerStateMachine playerStateMachine = player.GetComponent<PlayerStateMachine>();
                playerStateMachine._iMouvementLockedWriter.isMouvementLocked = true;


            }
        }
        else
        {
            for (int i = 0; i < managerManager.Players.Count; i++)
            {
                var StateMachine = players[i]._playerStateMachine;
                StateMachine.ChangeState(StateMachine.stateIdle);
                StateMachine.gameObject.transform.position = spawnpoints[i].transform.position;
                StateMachine._iMouvementLockedWriter.isMouvementLocked = true;
            }
        }
        #endregion
    }
    bool ShouldEndRound()
    {
        foreach (var player in alivePlayers.Skip(1))
        {
            if (player._team != alivePlayers[0]._team)
            {
                return false;
            }
        }
        return true;
    }
    public void RoundEnd()
    {
        foreach (Player player in alivePlayers)
        {
            player._points += ManagerManager.Instance.gameParams.PointsPerRound;
        }
    }
    public IEnumerator NewRound()
    {
        CameraTransition.Instance.FreezeIt();
        alivePlayers = new List<Player>(players);
        var allboules = FindObjectsOfType<BouleMouvement>();
        for(int i = 0; i < players.Count; i++)
        {
            allboules[0].resetChangeScene();
        }
        var scenes = ManagerManager.Instance.gameParams.Scenes;
        string sceneName = scenes[Random.Range(0, scenes.Length-1)];
        var asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.roundState);
    }
    public void KillPlayer(PlayerStateMachine playerKilled)
    {
        Player player = players.Find(x => x._playerStateMachine == playerKilled);
        alivePlayers.Remove(player);

        if (ShouldEndRound())
        {
            RoundEnd();
            GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.endRound);
        }
    }
    public void UpdateScores()
    {
        print("updated");
        for (int i = 0; i < players.Count; i++)
        {
            int eValue = (int)players[i]._team;
            scores[eValue].text = players[i]._points.ToString();
        }
    }
    public void ShowCadrants()
    {
        print("Shown");
        for (int i = 0; i < players.Count; i++)
        {
            int eValue = (int)players[i]._team;
            cadrants[eValue].SetActive(true);
            if(scores[eValue].text == "")
            {
                scores[eValue].text = players[i]._points.ToString();
            }
        }
    }

    [Button]
    public void EndRoundTest()
    {
        RoundEnd();
        GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.endRound);
    }
}

