using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BouleParams", order = 1)]

public class BouleParams : ScriptableObject
{ 
    [Header("rotation de la boule")]
    [Space(10)]
    [Header("Vitesse de rotation de la boule autour du joueur")]
    [Space(5)]
    public float _rotationSpeed = 100.0f; // Vitesse de rotation de la boule autour du joueur
    [Space(10)]

    [Header("Sens de rotation initial (true va de gauche � droite)")]
    [Space(5)]
    public bool _clockwise = true; // Sens de rotation initial
    [Space(15)]

    [Header("Lancé de la boule")]
    [Space(10)]

    [Header("vitesse � laquelle la boule va revenir en place apr�s un bug de d�placement")]
    [Space(5)]
    [Range(0, 9999)]
    public float _resetSpeed = 400;
    [Space(10)]

    [Header("Vitesse de lancer de la boule")]
    [Space(5)]
    [Range(0, 9999)]
    public float _speedThrowing = 10.0f;
    [Space(10)]

    [Header("Vitesse du lancer de la boule")]
    [Range(0, 1000)]
    public float _speedBack = 2.0f;
    [Space(10)]

    [Header("Combien de temps avant que la boule ai sa vitesse max au retour (en seconde)")]
    [Space(5)]
    public float _lerpDurationFast = 2.0f;
    [Space(10)]

    [Header("Combien de temps avant que la boule ai fini de ralentire (en seconde)")]
    [Space(5)]
    public float _lerpDurationSlow = 2.0f;
    [Space(10)]

    [Header("Combien de temps avant que la boule revienne (en seconde)")]
    [Space(5)]
    public float _retourTimeMax = 2.0f;
    [Space(10)]

    [Header("la courbe de vitesse que va prendre la boule (en terme de vitesse)")]
    [Space(5)]
    public AnimationCurve _lerpCurve;
    [Space(15)]

    [Header("Distance entre la boule et le player ")]
    [Space(5)]
    public float _size = 0;
    [Space(15)]

    [Header("Les distances des boules")]
    [Space(10)]
    [Header("la distances acceptable lorsque la boule reviens et va rebondire sur les endroits sur lesquelles elle avait rebondi")]
    [Space(5)]
    [Range (0, 50)]
    public float _distancePoints = 0.5f;
    [Space(15)]
    [Header("la distances acceptable lorsque la boule est assez proche dans l'update (ca check au cas ou la boule est coicé sur une platform)")]
    [Space(5)]
    [Range(0, 5)]
    public float _distanceCloseEneaughtUpdate = 0.25f;
    [Space(15)]
    [Header("la distances non-acceptable lorsque la boule est trop loin dans l'update (ca check au cas ou la boule est coicé sur une platform)")]
    [Space(5)]
    [Range(0, 6)]
    public float _distanceTooFarUpdate = 0.5f;


}

