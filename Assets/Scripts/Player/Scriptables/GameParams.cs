using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameParams", order = 1)]
public class GameParams : ScriptableObject
{
    [Header("Game Settings")]
    [SerializeField]
    public int NombreJoueurs = 4;
}
