using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerClass : MonoBehaviour
{
    public PlayerInput _playerInputs;
    public enum Team
    {
        blue,
        red,
        green,
        yellow
    }
    public int _points = 0;
    public bool _isAlive = true;
    void Start()
    {
        RoundManager.Instance.addPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
