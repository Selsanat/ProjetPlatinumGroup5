using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameParams", order = 1)]
public class GameParams : ScriptableObject
{
    [Header("Game Settings")]
    [SerializeField]
    public int NombreJoueurs = 4;
    [SerializeField]
    public int NombreRounds = 4;
    [SerializeField]
    public int PointsToWin = 4;
    [SerializeField]
    public int PointsPerRound = 1;

    [SerializeField]
    public float TransiTimeAfterRound = 1;

    [SerializeField]
    public SceneAsset[] Scenes;
}
