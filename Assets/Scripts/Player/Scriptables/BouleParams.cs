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
    public float _resetSpeed = 400;
    [Space(10)]

    [Header("Vitesse de lancer de la boule")]
    [Space(5)]
    public float _speedThrowing = 10.0f;
    [Space(10)]

    [Header("Vitesse du lancer de la boule")]
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

    [Header("Distance entre la boule et le player (c'est une multiplication donc ca va vite)")]
    [Space(5)]
    public float _size = 1.0f;
     
}

