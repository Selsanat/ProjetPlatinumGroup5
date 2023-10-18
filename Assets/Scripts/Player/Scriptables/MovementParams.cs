using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MovementParams", order = 1)]
public class MovementParams : ScriptableObject
{

    // Les paramètres et leurs range ont été définit sur demande des GDs
    #region Jump

    [Header("Jump")]

    [Range(0, 0.3f)]
    [Tooltip("Force de saut")] public float jumpMaxHeight = 0.5f;
    [Range(0, 5)]
    [Tooltip("Time to reach jumpheight")] public float jumpDuration = 1;
    [Range(0, 0.3f)]
    [Tooltip("Saut Min")] public float minJump = 0;
    [Range(0, 2)]
    [Tooltip("Temps alloué au joueur pour sauter alors qu'il n'est plus sur la plateforme")]public float coyoteWindow = 0;
    [Range(0, 1)]
    [Tooltip("% of jumpforce subtracted to run speed")] public float inertieLoss = 0.15f;

    [Range(0, 2)]
    [Tooltip("Air Acceleration time")] public float JumpAccelerationTime = 1;
    [Range(0, 2)]
    [Tooltip("Air max Speed X")] public float airMaxSpeedX = 1;

    #endregion

    #region Fall

    [Header("Fall")]
    [Range(0, 2)]
    [Tooltip("Fall speed")]public float fallDuration = 5;
    [Range(0, 2)]
    [Tooltip("Fall speed maximale")]public float maxFallSpeed = 5;
    [Range(0, 2)]
    [Tooltip("Fall speed acceleration time")] public float fallAccelerationTime = 1;
    [Range(0, 2)]
    [Tooltip("fall max X Speed")] public float fallMaxSpeedX = 1;

    #endregion

    #region Run
    [Header("Run")]
    [Range(0, 2)]
    [Tooltip("Vitesse maximale horizontale du joueur")]public float maxSpeed = 1;
    [Range(0, 2)]
    [Tooltip("Temps que prend le joueur a Accélerer")]public float accelerationTime = 2;
    [Range(0, 2)]
    [Tooltip("Temps que prend le joueur a Décélérer")]public float decelerationTime = 2;

    [Range(0, 2)]
    [Tooltip("Temps que prend le joueur a Accélerer")]public float turnAccelerationTime = 2;
    [Range(0, 2)]
    [Tooltip("Temps que prend le joueur a Décélérer")] public float turnDecelerationTime = 2;

    [Range(0, 2)]
    [Tooltip("A quel point le joueur peut prendre une pente pentue")] public float slideSlopeThresHold = 2;

    #endregion


}
