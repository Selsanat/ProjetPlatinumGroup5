using Lofelt.NiceVibrations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HapticsManager : MonoBehaviour
{


    public static HapticsManager Instance;
    public RumblePresets[] PresetsForRumble;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void Vibrate (string name, Gamepad gamepad)
    {
        GamepadRumbler.Init();
        GamepadRumble preset = Array.Find(PresetsForRumble, p => p.name == name).rumble;
        GamepadRumbler.SetCurrentGamepad(gamepad.deviceId);
        GamepadRumbler.Load(preset);
        GamepadRumbler.Play();
    }
    [System.Serializable]
    public struct RumblePresets
    {
        public string name;
        public GamepadRumble rumble;
    }
}
