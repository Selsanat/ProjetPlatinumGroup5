using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Game Manager Singleton

    public static GameManager Instance;
    private int _playerAlive = 0;
    public Transform[] spawnPoints;
    public GameObject playerPrefabs;
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
    }
    public void playerDied()
    {
        _playerAlive--;
        if (_playerAlive <= 1)
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
        /*if (_playerAlive != 4)
            return;*/

        
    }
    public void setUpRound()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        PlayerStateMachine playerStateMachine = players[_playerAlive - 1].GetComponent<PlayerStateMachine>();
        playerStateMachine.ChangeState(playerStateMachine.stateIdle);
        playerStateMachine.velocity = Vector2.zero;
        playerStateMachine.transform.position = spawnPoints[_playerAlive - 1].position;
        playerStateMachine.CurrentState.LockMouvement();
    }

    public void addPlayer()
    {
        _playerAlive++;
        setUpRound();
    }



    
}
