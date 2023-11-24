using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;

public class PlayerInput : MonoBehaviour
{

    #region SetUpInputs

    public InputActionAsset _ipaPlayercontrols;
    private List<InputAction> _iaActions;
    private Pause pause;
    public IOrientWriter IOrient;
    public IWantsJumpWriter jump;
    public float triggers;
    public float shoulders;
     
    #endregion

    #region InitInputs
    void EnableInputAction(bool shouldActivate)
    {
        foreach (InputAction action in _iaActions)
        {
            if (shouldActivate) action.Enable();
            else action.Disable();
        }
    }

    public void SetupInputs()
    {
        //Setup Liste des inputs
        pause = FindAnyObjectByType<Pause>();   

        _iaActions = new List<InputAction>();
        foreach (var action in _ipaPlayercontrols.actionMaps[0].actions)
        {
            _iaActions.Add(action);
        }
    }
    private void OnEnable()
    {
        if(_iaActions != null)
        EnableInputAction(true);
    }
    private void OnDisable()
    {
        EnableInputAction(false);
    }
    private void Start()
    {
        SetupInputs();
    }
    #endregion
    void FixedUpdate()
    {
        IOrient.orient = _iaActions[0].ReadValue<Vector2>();
        if (IOrient.orient.y > 0)
        {
            jump.wantsJump = true;
        }
        else jump.wantsJump = false;
        if (_iaActions[2].ReadValue<float>() > 0)
        {
            pause.onPause();
        }
        Vector2 gachettes = _iaActions[1].ReadValue<Vector2>();
        triggers = gachettes.y;
        shoulders = gachettes.x;
        //_iaActions[1].performed += ctx => SceneManager.LoadScene("LeandroMenu");

    }

}
 