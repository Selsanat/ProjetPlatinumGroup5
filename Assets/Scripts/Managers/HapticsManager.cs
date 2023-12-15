using Lofelt.NiceVibrations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

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
        LofeltHaptics.Initialize();
        GamepadRumbler.Init();
    }

   public  void Vibrate (string name, Gamepad gamepad)
    {
        StartCoroutine(VibrateCoroutine(name, gamepad));
    }
    IEnumerator VibrateCoroutine(string name, Gamepad gamepad)
    {
        GamepadRumble preset = Array.Find(PresetsForRumble, p => p.name == name).rumble;
        gamepad.SetMotorSpeeds(preset.lowFrequencyMotorSpeeds[0], preset.highFrequencyMotorSpeeds[0]);
        yield return new WaitForSeconds(preset.durationsMs[0]/1000f);
        gamepad.SetMotorSpeeds(0, 0);
    }

    [System.Serializable]
    public struct RumblePresets
    {
        public string name;
        public GamepadRumble rumble;
    }
}
