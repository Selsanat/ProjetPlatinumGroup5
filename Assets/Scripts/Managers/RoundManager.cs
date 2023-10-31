using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private int _nbPlayer = 0;
    private GameObject[] _spawnPoints;
    public GameObject _playerPrefabs;
    public static RoundManager Instance;
    private List<playerClass> _players;
    private List<playerClass> _playersAlive;
    [SerializeField]
    private int _pointsRound;
    [SerializeField]
    private int _pointsWin;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
    }
    public void roundOver()
    {
        print("round over");
        foreach (playerClass player in _playersAlive) 
        {
            player._points += _pointsRound;
        }
        foreach (playerClass player in _players)
        {
            if(player._points >= _pointsWin)
            {
                print("player win");
                return;
            }
        }
        startRound();
    }
    public void playerDied(playerClass player)
    {
        _nbPlayer--;
        _playersAlive.Remove(player);

        if (_nbPlayer <= 1)
        {
            roundOver();
        }
    }

    private void Start()
    {
        startRound();
    }
    public void startRound()
    {
        if (_playersAlive.Count != 4)
            return;

        foreach (playerClass player in _players)
        {
            player.GetComponent<PlayerStateMachine>().CurrentState.UnlockMouvement();
        }

    }

    public void setUpRound()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoints");
        _playersAlive.Clear();
        
        foreach (playerClass player in _players)
        {
            player._isAlive = true;
            _playersAlive.Add(player);
            PlayerStateMachine playerStateMachine = player.GetComponent<PlayerStateMachine>();
            playerStateMachine.ChangeState(playerStateMachine.stateIdle);
            playerStateMachine.velocity = Vector2.zero;
            playerStateMachine.transform.position = _spawnPoints[_nbPlayer - 1].transform.position;
            playerStateMachine.CurrentState.LockMouvement();
        }
        
    }

    public void addPlayer(playerClass player)
    {
        _players.Add(player);
        _nbPlayer++;
        setUpRound();
    }
}
