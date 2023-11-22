using System.Reflection;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CameraParams", order = 1)]
public class CameraParams : ScriptableObject
{
    [Header("Transition")]
    public int numberOfJumps = 1;
    [Range(0f, 1000f)]
    public float jumpForce = 500;
    [Range(0f, 2000f)]
    public float heighOfFall = 500;
    [Range(0.1f, 1f)]
    public float jumpDuration = 0.1f;

    [Header("ZoomInRoundStart")]
    [Range(0.5f, 5f)]
    public float timeToMoveFromPlayerToPlayer = 1f;
    [Range(0.5f, 15f)]
    public float Zoom = 5f;
    [Range(0.5f, 5f)]
    public float TimeToZoom = 1f;
    [Range(0.5f, 5f)]
    public float intervalBetweenPlayers = 1f;
    [Range(0.5f, 5f)]
    public float timeAfterZoomsBeforeRoundStart = 1f;

    [Header("Camera")]
    public float cameraXmax = 1f;
    public float cameraYmax = 1f;
    public float ZpourSeReperer = 1f;
    [Range(0f, 5f)]
    public float CameraFollowSmoothness = 1f;

    [Header("RoundEnd")]
    [Range(0f, 25f)]
    public float distanceZAlaCam = 10f;
    [Range(0f, 2f)]
    public float SmoothnessZoomRoundEnd = 0.1f;
    [Range(0f, 15f)]
    public float OrthoSizeRoundEnd = 5f;
    [Range(0f, 5f)]
    public float TimeToZoomEndRound = 1f;
}
