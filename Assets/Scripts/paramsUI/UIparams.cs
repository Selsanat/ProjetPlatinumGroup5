using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIparams : MonoBehaviour
{
    public Slider _slider;
    public AudioMixer _audioMixer;
    public void changeValue()
    {
        _audioMixer.SetFloat("Volume", _slider.value);
        
    }
    private void OnEnable()
    {
        _slider.Select();
    }
    
}
