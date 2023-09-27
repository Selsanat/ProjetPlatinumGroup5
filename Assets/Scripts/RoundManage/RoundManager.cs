using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private enum RoundState
    {
        SoloRound,
        DuoRound,
        TrioRound
    }
    [SerializeField]
    private int _roundBeforeTeamRound = 3;
    [SerializeField]
    private RoundState _roundState = RoundState.SoloRound;
    [SerializeField]
    private int _roundNumber = 0;
    [SerializeField]
    private int _aliveCount = 0;
    [SerializeField]
    private bool _isTeam1Dead = false;
    [SerializeField]
    private bool _isTeam2Dead = false;
    [SerializeField]
    private GameObject[] _players;
    [SerializeField]
    private List<GameObject> _team1, _team2, _playerAvailable;

    private Timer _timer;

    public bool _isPlayer1Dead = false;
    public bool _isPlayer2Dead = false;
    public bool _isPlayer3Dead = false;
    public bool _isPlayer4Dead = false;


    private void Start()
    {
        _timer = FindAnyObjectByType<Timer>();
    }
    public void checkWin()
    {
        if(_roundState == RoundState.SoloRound) // 1v1v1v1
        {
            if (_aliveCount == 1)
            {
                newRoud();
                _timer.StopTimer();
            }
        }
        else if(_roundState == RoundState.DuoRound) // 2v2
        {
            if (_isTeam1Dead)
            {
                newRoud();
                _timer.StopTimer();

            }
            else if (_isTeam2Dead)
            {
                newRoud();
                _timer.StopTimer();

            }
        }
        else if(_roundState == RoundState.TrioRound) //1v3
        {
            if (_isTeam1Dead) //team 1 toujours solo
            {
                newRoud();
                _timer.StopTimer();

            }
            else if (_isTeam2Dead) //team 2 toujours trio
            {
                newRoud();
                _timer.StopTimer();

            }
        }
    }

    public void newRoud()
    {
        _roundNumber++;
        _isPlayer1Dead = false;
        _isPlayer2Dead = false;
        _isPlayer3Dead = false;
        _isPlayer4Dead = false;
        _isTeam1Dead = false;
        _isTeam2Dead = false;
        _aliveCount = 0;
        _playerAvailable.Clear();
        _team1.Clear();
        _team2.Clear();

        _playerAvailable = new List<GameObject>(_players);

        if(_roundNumber >= _roundBeforeTeamRound)
            choiceTypeRound();

        _timer.StartTimer();


    }

    public void choiceTypeRound()
    {
        int random = Random.Range(0, 3);
        if (random == 0)
        {
            _roundState = RoundState.SoloRound;
            foreach (GameObject player in _players)
            {
                player.tag = "player";
            }
        }
        else if (random == 1)
        {
            _roundState = RoundState.DuoRound;
            for (int i = 1; i <= 4; i++) // 2 joueurs par équipe
            {
                int randomIndex = Random.Range(0, _playerAvailable.Count);
                if (i <= 2)
                {
                    _playerAvailable[randomIndex].tag = "team1";
                    _team1.Add(_playerAvailable[randomIndex]);
                    _playerAvailable.RemoveAt(randomIndex);
                }
                else
                {
                    _playerAvailable[randomIndex].tag = "team2";
                    _team2.Add(_playerAvailable[randomIndex]);
                    _playerAvailable.RemoveAt(randomIndex);
                }

            }



        }

        else if (random == 2)
        {
            _roundState = RoundState.TrioRound;
            GameObject playerMostPoint = null;
            foreach (GameObject player in _players)
            {
                if(playerMostPoint == null)
                {
                    playerMostPoint = player;
                    
                }
                /*else if(player._point > playerMostPoint._point)
                {
                    playerMostPoint.tag = "team2";
                    _team2.Add(playerMostPoint);

                    playerMostPoint = player;
                }*/
                else
                {
                    player.tag = "team2";
                    _team2.Add(player);
                }
            }
            playerMostPoint.tag = "team1";
            _team1.Add(playerMostPoint);




        }
    }
}
