using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float _countdownTime = 180.0f; // 3 minutes en secondes
    private bool _isTimerRunning = false;
    public TextMeshProUGUI _timerText;


    private void Update()
    {
        if (_isTimerRunning)
        {
            _countdownTime -= Time.deltaTime;
            if (_countdownTime <= 0)
            {
                Debug.Log("Temps écoulé !");
                _timerText.text = "0:0";

                StopTimer();
            }
            else
            {
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

    public void StopTimer()
    {
        _isTimerRunning = false;
    }
}
