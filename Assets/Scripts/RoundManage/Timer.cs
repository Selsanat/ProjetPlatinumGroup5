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
        // D�marrez le timer au d�but (peut �galement �tre d�clench� par un �v�nement ou une action)
        StartTimer();
    }
    private void Update()
    {
        // V�rifiez si le timer est en cours d'ex�cution
        if (_isTimerRunning)
        {
            // R�duire le temps restant
            _countdownTime -= Time.deltaTime;
            // V�rifiez si le temps est �coul�
            if (_countdownTime <= 0)
            {
                // Le temps est �coul�, ex�cutez votre action ici (par exemple, affichez un message)
                Debug.Log("Temps �coul� !");
                // Arr�tez le timer
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

    // Arr�tez le timer
    public void StopTimer()
    {
        _isTimerRunning = false;
    }
}
