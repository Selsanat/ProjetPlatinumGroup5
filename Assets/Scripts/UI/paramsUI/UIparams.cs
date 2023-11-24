using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class UIparams : MonoBehaviour
{
    public Slider _slider;
    public Button _quit;
    public AudioMixer _audioMixer;
    public InputSystemUIInputModule _inputSystemUIInputModule;
    private void Start()
    {
        _slider.value = PlayerPrefs.GetFloat("Volume", 0);
    }
    public void changeValue()
    {
        _audioMixer.SetFloat("Volume", _slider.value);
        
    }
    private void Update()
    {
        if (_inputSystemUIInputModule.move.action.ReadValue<Vector2>().y == -1 && EventSystem.current.currentSelectedGameObject.gameObject != _quit.gameObject)
        {
            _quit.Select();
        }
        else if(_inputSystemUIInputModule.move.action.ReadValue<Vector2>().y == 1 && EventSystem.current.currentSelectedGameObject.gameObject != _slider.gameObject)
        {
            _slider.Select();
        }
    }

}
