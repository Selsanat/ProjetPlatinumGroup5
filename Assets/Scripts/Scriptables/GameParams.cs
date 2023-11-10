
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameParams", order = 1)]
public class GameParams : ScriptableObject
{
    [Header("Game Settings")]
    [SerializeField]
    public int PointsToWin = 4;
    [SerializeField]
    public int PointsPerRound = 1;

    [SerializeField]
    public float TransiTimeAfterRound = 1;

    [SerializeField]
    public string[] Scenes;

    [HideInInspector]
    public Object[] objects = new Object[1];
}
