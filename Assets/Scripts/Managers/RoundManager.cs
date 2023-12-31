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
using DG.Tweening;
using Highlighters;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RoundManager : MonoBehaviour
{

    public static RoundManager Instance;
    public List<Player> players = new List<Player>();
    public List<Player> alivePlayers = new List<Player>();
    private GameParams _gameParams => ManagerManager.Instance.gameParams;
    private ManagerManager managerManager => ManagerManager.Instance;
    private InputsManager inputsManager => InputsManager.Instance;
    public TMP_Text[] scores;
    public GameObject[] cadrants;
    public Volume Volume => ManagerManager.Instance.Volume;
    public ChromaticAberration CA;

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
            
            //playerStateMachine._playerStateMachine.GetComponentInChildren<SpriteRenderer>().color = RoundManager.Instance.teamColors[(int)_team];
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
            int[] teams = new int[4];
            for (int i = 0; i < managerManager.Players.Count; i++)
            {
                var player = InputsManager.Instance._playerInputManager.JoinPlayer(-1, -1, null, managerManager.Players.Keys.ToList()[i]);
                player.transform.position = spawnpoints[i].transform.position;
                PlayerStateMachine playerStateMachine = player.GetComponent<PlayerStateMachine>();
                playerStateMachine._iMouvementLockedWriter.isMouvementLocked = true;

                foreach (Animator animator in playerStateMachine.GetComponentsInChildren<Animator>())
                {
                    int team = (int)managerManager.Players.Values.ToList()[i];
                    animator.SetFloat("Blend", team);
                    playerStateMachine.team = team;
                }

                foreach (SpriteRenderer spriteRenderer in player.GetComponentsInChildren<SpriteRenderer>())
                {

                    spriteRenderer.color *= 1 - 0.35f * teams[playerStateMachine.team];
                    Color c = spriteRenderer.color;
                    spriteRenderer.color = new Color(c.r, c.g, c.b, 1);
                }
                List<Color> highlightsColors = new List<Color>()
                {
                Color.blue,
                Color.yellow,
                Color.red,
                Color.green
                };
                foreach (Highlighter h in playerStateMachine.GetComponentsInChildren<Highlighter>())
                {
                    h.Settings.OuterGlowColorFront = highlightsColors[playerStateMachine.team];
                }
                teams[playerStateMachine.team] += 1;
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
                StateMachine.bouleMouvement.gameObject.SetActive(true);
                Animator animator = StateMachine.bouleMouvement.GetComponentInChildren<Animator>();
                int team = (int)managerManager.Players.Values.ToList()[i];
                animator.SetFloat("Blend", team);
                StateMachine.team = team;
                StateMachine.bouleMouvement.resetChangeScene();
            }
        }
        #endregion
        Volume.profile.TryGet<ChromaticAberration>(out CA);
    }
    public bool ShouldEndRound()
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
        SoundManager.instance.PlayClip("Round Win");
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
        for(int i = 0; i < allboules.Length; i++)
        {
            allboules[i].resetChangeScene();
        }
        Random.InitState(System.DateTime.Now.Millisecond);
        var scenes = ManagerManager.Instance.gameParams.Scenes;
        string sceneName = scenes[Random.Range(0, scenes.Length-1)];
        while(SceneManager.GetActiveScene().name == sceneName)
        {
            sceneName = scenes[Random.Range(0, scenes.Length-1)];
        }
        var asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        CA.intensity.value = 0;
        GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.roundState);
    }
    public void KillPlayer(PlayerStateMachine playerKilled)
    {
        CameraTransition.Instance.CameraShake();
        Player player = players.Find(x => x._playerStateMachine == playerKilled);
        alivePlayers.Remove(player);
        SoundManager.instance.PlayClip("death");
        if (alivePlayers.Count == 2) SoundManager.instance.AddPist(4);
        else
        if (ShouldEndRound())
        {
            RoundEnd();
            GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.endRound);
        }
    }
    public void UpdateScores()
    {
        int[] scorelist = new int[4];
        for (int i = 0; i < players.Count; i++)
        {
            int eValue = (int)players[i]._team;
            scorelist[eValue]+= players[i]._points;
            scores[eValue].text =""+scorelist[eValue];
        }
    }
    public void ShowCadrants()
    {
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
    public void DestroyAllPlayers()
    {
        foreach(Player player in players)
        {
            
            Destroy(player._playerStateMachine.gameObject);
        }
        players.Clear();
    }

    [Button]
    public void EndRoundTest()
    {
        RoundEnd();
        SoundManager.instance.PlayClip("Win");
        GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.endRound);
    }

    [Button]
    public void killTwo()
    {
        KillPlayer(alivePlayers[0]._playerStateMachine);
        KillPlayer(alivePlayers[1]._playerStateMachine);
    }
}

