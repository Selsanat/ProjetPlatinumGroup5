using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float _countdownTime = 180.0f; // 3 minutes en secondes
    private bool _isTimerRunning = false;
    public TextMeshProUGUI _timerText;

    private void Start()
    {
        // Démarrez le timer au début (peut également être déclenché par un événement ou une action)
        StartTimer();
    }
    private void Update()
    {
        // Vérifiez si le timer est en cours d'exécution
        if (_isTimerRunning)
        {
            // Réduire le temps restant
            _countdownTime -= Time.deltaTime;
            // Vérifiez si le temps est écoulé
            if (_countdownTime <= 0)
            {
                // Le temps est écoulé, exécutez votre action ici (par exemple, affichez un message)
                Debug.Log("Temps écoulé !");
                // Arrêtez le timer
                _timerText.text = "0:0";

                StopTimer();
            }
            else
            {
                // Calcul des minutes et des secondes restantes
                int minutes = Mathf.FloorToInt(_countdownTime / 60);
                int seconds = Mathf.FloorToInt(_countdownTime % 60);
                _timerText.text = minutes + ":" + seconds;
            }
        }
    }
    public void StartTimer()
    {
        _isTimerRunning = true;
    }

    // Arrêtez le timer
    public void StopTimer()
    {
        _isTimerRunning = false;
    }
}
