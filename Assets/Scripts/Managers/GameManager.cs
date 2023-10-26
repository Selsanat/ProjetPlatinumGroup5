using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Game Manager Singleton

    public static GameManager Instance;
    private float _playerAlive = 4;
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

    public void startRound()
    {
        _playerAlive = 4;
    }
}
