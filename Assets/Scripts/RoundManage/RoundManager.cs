using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class Player : MonoBehaviour
{
    public enum PlayerTeam // a mettre dans le player, et le mettre de base a available
    {
        Team1 = 0,
        Team2 = 1,
        Team3 = 2,
        Team4 = 3,
        Available = 4
    }
    public PlayerTeam _playerTeam = PlayerTeam.Available;

    public bool _isDead = false;
    public int _points = 0;



}
public class RoundManager : MonoBehaviour
{
    
    private enum RoundState
    {
        SoloRound,
        DuoRound,
        TrioRound,
        DuoSoloRound
    }
    private Typeround _typeRound;

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
    private GameObject[] AliveDead;
    [SerializeField]
    private GameObject[] _players;
    [SerializeField]
    private List<GameObject> _team1, _team2, _playerAvailable;
    [SerializeField]

    private Timer _timer;


    public bool _isPlayer1Dead = false;
    public bool _isPlayer2Dead = false;
    public bool _isPlayer3Dead = false;
    public bool _isPlayer4Dead = false;

    struct Typeround
    {
        public int[] solo, duo, trio, duoSolo;
    }
    private void Start()
    {
        _timer = FindAnyObjectByType<Timer>();

        _typeRound.solo = new int[] { 1, 1, 1, 1 };
        _typeRound.duo = new int[] { 2, 2 };
        _typeRound.trio = new int[] { 3, 1 };
        _typeRound.duoSolo = new int[] { 2, 1, 1 };
    }
    public void testCheckWin()
    {
        //soit avoir des liste qui refer si dans chaque team il y a des morts
        foreach (var player in _players) 
        {
            if (player.GetComponent<Player>()._isDead)
            {
                switch (player.GetComponent<Player>()._playerTeam)
                {
                    case Player.PlayerTeam.Team1:
                        _isPlayer1Dead = true;
                        break;
                    case Player.PlayerTeam.Team2:
                        _isPlayer2Dead = true;
                        break;
                    case Player.PlayerTeam.Team3:
                        _isPlayer3Dead = true;
                        break;
                    case Player.PlayerTeam.Team4:
                        _isPlayer4Dead = true;
                        break;
                }
            }
        }

    }

    public void checkWin()
    {
        if (_roundState == RoundState.SoloRound) // 1v1v1v1
        {
            if (_aliveCount == 1)
            {
                newRoud();
                _timer.StopTimer();
            }
        }
        else if (_roundState == RoundState.DuoRound) // 2v2
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
        else if (_roundState == RoundState.TrioRound) //1v3
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
        else if (_roundState == RoundState.DuoSoloRound) //2v1v1
        {
            if (_aliveCount == 1) // toutes les �quipes peuvent win ici
            {
                newRoud();
                _timer.StopTimer();
            }
            else if (_aliveCount == 2 && !_team1[0].GetComponent<Player>()._isDead && !_team1[1].GetComponent<Player>()._isDead) // s'il y a deux survivant et qu'ils sont de la team 1
            {
                newRoud();
                _timer.StopTimer();
            }
        }
    }

    public void newRoud()
    {
        _roundNumber++;
        _isTeam1Dead = false;
        _isTeam2Dead = false;
        _aliveCount = 0;
        _playerAvailable.Clear();
        _team1.Clear();
        _team2.Clear();

        _playerAvailable = new List<GameObject>(_players);

        if (_roundNumber >= _roundBeforeTeamRound)
            test(_typeRound.solo);

        _timer.StartTimer();


    }

    public void test(int[] TypeRound)
    {

        foreach (int i in TypeRound)
        {
            for (int j = 0; j <= i; j++)
            {
                int random = Random.Range(0, _players.Length);
                Player.PlayerTeam playerTeam = (Player.PlayerTeam)i;
                while (_players[random].GetComponent<Player>()._playerTeam != Player.PlayerTeam.Available)
                {
                    random = Random.Range(0, _players.Length);
                }
                _players[random].GetComponent<Player>()._playerTeam = playerTeam;
            }
        }
    }

    //sort order by d un parametre 


    public List<GameObject> sortingPlayerPoints(GameObject[] players)
    {
        List<GameObject> sortPlayer = new List<GameObject>(players.Length);
        foreach (GameObject p in players)
            sortPlayer.Add(p);

        sortPlayer = players.OrderBy(x => x.GetComponent<Player>()._points).ToList();
        return sortPlayer;
    }

    /*public void choiceTypeRound()
    {
        int random = Random.Range(0, 4);

        switch (random)
        {
            case 0:// 1v1v1v1
                _roundState = RoundState.SoloRound;
                foreach (GameObject player in _players)
                {
                    player.tag = "Player";
                }
                break;

            case 1: // 2v2
                _roundState = RoundState.DuoRound;
                for (int i = 1; i <= 4; i++) // 2 joueurs par �quipe
                {
                    int randomIndex = Random.Range(0, _playerAvailable.Count);
                    if (i <= 2)
                    {
                        _playerAvailable[randomIndex].tag = "Team1";
                        _team1.Add(_playerAvailable[randomIndex]);
                        _playerAvailable.RemoveAt(randomIndex);
                    }
                    else
                    {
                        _playerAvailable[randomIndex].tag = "Team2";
                        _team2.Add(_playerAvailable[randomIndex]);
                        _playerAvailable.RemoveAt(randomIndex);
                    }

                }
                break;

            case 2:// 1v3
                _roundState = RoundState.TrioRound;
                GameObject playerMostPoints = null;
                foreach (GameObject player in _players)
                {

                    if (playerMostPoints == null || player._point > playerMostPoints._point)
                    {
                        playerMostPoints.tag = "Team2";
                        _team2.Add(playerMostPoints);

                        playerMostPoints = player;
                    }
                    else
                    {
                        player.tag = "Team2";
                        _team2.Add(player);
                    }
                }
                playerMostPoints.tag = "Team1";
                _team1.Add(playerMostPoints);
                break;

            case 3: // 2v1v1
                _roundState = RoundState.DuoSoloRound;


                int randomDuo = Random.Range(0, 3);
                switch (randomDuo)
                {
                    case 0: //plus fort avec le plus faible
                        GameObject playerMostPoint = null;
                        GameObject playerLeastPoint = null;

                        foreach (GameObject player in _players)
                        {


                            if (playerMostPoint == null || player.points > playerMostPoint.points)
                            {
                                playerMostPoint = player;
                            }

                            if (playerLeastPoint == null || player.points < playerLeastPoint.points)
                            {
                                playerLeastPoint = player;
                            }
                        }

                        playerMostPoint.tag = "Team1";
                        _playerAvailable.Remove(playerMostPoint);
                        _team1.Add(playerMostPoint);
                        playerLeastPoint.tag = "Team1";
                        _playerAvailable.Remove(playerLeastPoint);
                        _team1.Add(playerLeastPoint);

                        foreach (GameObject player in _playerAvailable)
                        {
                            player.tag = "Player";
                        }
                        break;

                    case 1: //plus fort avec le plus fort
                        playerMostPoint = null;
                        GameObject secondPlayerMostPoints = null;

                        foreach (GameObject player in _players)
                        {


                            if (playerMostPoint == null || player.points > playerMostPoint.points)
                            {
                                playerMostPoint = player;
                            }

                        }
                        foreach (GameObject player in _players)
                        {


                            if (secondPlayerMostPoints == null || player.points > secondPlayerMostPoints.points && player != playerMostPoint)
                            {
                                secondPlayerMostPoints = player;
                            }

                        }
                        playerMostPoint.tag = "Team1";
                        secondPlayerMostPoints.tag = "Team1";
                        _team1.Add(playerMostPoint);
                        _team1.Add(secondPlayerMostPoints);

                        _playerAvailable.Remove(playerMostPoint);
                        _playerAvailable.Remove(secondPlayerMostPoints);

                        foreach (GameObject player in _playerAvailable)
                        {
                            player.tag = "Player";
                        }
                        break;

                    case 2: //plus faible avec le plus faible
                        playerMostPoint = null;
                        secondPlayerMostPoints = null;

                        foreach (GameObject player in _players)
                        {


                            if (playerMostPoint == null || player.points > playerMostPoint.points)
                            {
                                playerMostPoint = player;
                            }

                        }
                        foreach (GameObject player in _players)
                        {


                            if (secondPlayerMostPoints == null || player.points > secondPlayerMostPoints.points && player != playerMostPoint)
                            {
                                secondPlayerMostPoints = player;
                            }

                        }
                        playerMostPoint.tag = "Player";
                        secondPlayerMostPoints.tag = "Player";
                        _playerAvailable.Remove(playerMostPoint);
                        _playerAvailable.Remove(secondPlayerMostPoints);

                        foreach (GameObject player in _playerAvailable)
                        {
                            player.tag = "Team1";
                            _team1.Add(player);

                        }
                        break;

                    case 3: //millieu avec millieu
                        playerMostPoint = null;
                        playerLeastPoint = null;

                        foreach (GameObject player in _players)
                        {


                            if (playerMostPoint == null || player.points > playerMostPoint.points)
                            {
                                playerMostPoint = player;
                            }

                            if (playerLeastPoint == null || player.points < playerLeastPoint.points)
                            {
                                playerLeastPoint = player;
                            }
                        }

                        playerMostPoint.tag = "Player";
                        _playerAvailable.Remove(playerMostPoint);
                        playerLeastPoint.tag = "Player";
                        _playerAvailable.Remove(playerLeastPoint);

                        foreach (GameObject player in _playerAvailable)
                        {
                            player.tag = "Team1";
                            _team1.Add(player);

                        }
                        break;
                }
                break;
        }

    }*/
}