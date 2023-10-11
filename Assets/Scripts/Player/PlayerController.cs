using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private InputActionAsset _IPplayercontrols;
    private List<InputAction> _actions;
    private Vector2 _moveDirection = Vector2.zero;
    private Rigidbody _rb;
    public float moveSpeed = 8;
    public float jumpForce = 5;
    void Awake()
    {
        //_IPplayercontrols = this.GetComponent<PlayerInput>().actions;
        //for (int i = 0; i< _IPplayercontrols.actionMaps[0].actions.Count; i++)
        //{
        //    _actions.Add(_IPplayercontrols.actionMaps[0].actions[i]);
        //}
    }

    //void EnableInputAction(bool ShouldActivate)
    //{
    //    foreach(InputAction action in _actions)
    //    {
    //        if(ShouldActivate) action.Enable();
    //        else action.Disable();
    //    }
    //}
    //private void OnEnable()
    //{
    //    EnableInputAction(true);
    //    _actions[1].performed += Attack;
    //}
    //private void OnDisable()
    //{
    //    EnableInputAction(false);
    //}
    void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        //_moveDirection = _actions[0].ReadValue<Vector2>();
        _rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _rb.velocity.y);
        if (_moveDirection.y > 0) _rb.velocity = new Vector2(_rb.velocity.x, _moveDirection.y * jumpForce);

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }


    private void Attack(InputAction.CallbackContext context)
    {
        transform.DOShakeScale(1);
        transform.DOShakePosition(1);
    }


}