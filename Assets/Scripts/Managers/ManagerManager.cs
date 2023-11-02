using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerManager : MonoBehaviour
{
    public static ManagerManager Instance;
    public GameParams gameParams;
    public BouleParams bouleParams;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
