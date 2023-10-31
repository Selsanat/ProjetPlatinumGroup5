using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public enum Team
    {
        blue,
        red,
        green,
        yellow
    }
    public Color[] teamColors = new Color[4] { Color.blue, Color.red, Color.green, Color.yellow };
    public class Player
    {
        public PlayerInput _playerInputs;
        public PlayerStateMachine _playerStateMachine;
        public Team _team;
        public int _points = 0;

        public Player(InputsManager.PlayersInputs playerStateMachine)
        {
            _playerInputs = playerStateMachine._playerInputs;
            _playerStateMachine = playerStateMachine._playerStateMachine;
            _team = (Team)Team.GetValues(typeof(Team)).GetValue(RoundManager.Instance.players.Count);
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
            for (int i = 0; i < managerManager.gameParams.NombreJoueurs; i++)
            {
                print(devices.Count);
                var player = InputsManager.Instance._playerInputManager.JoinPlayer(-1, -1, null, devices[i]);
                player.transform.position = spawnpoints[i].transform.position;
                PlayerStateMachine playerStateMachine = player.GetComponent<PlayerStateMachine>();
                playerStateMachine._iMouvementLockedWriter.isMouvementLocked = true;


            }
        }
        else
        {
            print("ChangeScene");
            for (int i = 0; i < managerManager.gameParams.NombreJoueurs; i++)
            {
                players[i]._playerStateMachine.gameObject.transform.position = spawnpoints[i].transform.position;
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
        alivePlayers.Clear();
        var scenes = ManagerManager.Instance.gameParams.Scenes;
        string sceneName = scenes[Random.Range(0, scenes.Length)].name;
        StartCoroutine(NewRound(sceneName));
    }

    private static IEnumerator NewRound(string sceneName)
    {
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
}

