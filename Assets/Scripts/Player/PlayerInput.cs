using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{

    #region SetUpInputs

    public InputActionAsset _ipaPlayercontrols;
    private List<InputAction> _iaActions;
    public IOrientWriter IOrient;
    public IWantsJumpWriter jump;
     
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

        _iaActions[1].performed += ctx => SceneManager.LoadScene("LeandroMenu");

    }

}
 