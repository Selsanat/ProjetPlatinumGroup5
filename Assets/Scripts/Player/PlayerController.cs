using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    #region Inputs
    private InputActionAsset _IPplayercontrols;
    private List<InputAction> _actions;
    #endregion

    #region Physics
    [Header("Physics of player")]

    [Tooltip("Speed du joueur")] [SerializeField] float moveSpeed = 5;
    [Tooltip("Force de saut")] [SerializeField] float jumpForce = 5;
    [Tooltip("Fall speed")] [SerializeField] float gravityScale = 5;
    [Tooltip("Fall speed maximale")] [SerializeField] float maxFallSpeed = 5;
    [Tooltip("Vitesse maximale horizontale du joueur")] [SerializeField] float maxSpeed = 5;
    [Tooltip("Friction avec le sol après un mouvement. Low friction -> ice effect")] [SerializeField] float friction = 5;
    private Vector3 velocityDirection;
    #endregion

    #region RaycastDetection
    [Header("Collisions hitboxes (see gizmo)")]
    [SerializeField] bool seeGizmos = true;
    [SerializeField] RaycastParam rightCollisionRange;
    [SerializeField] RaycastParam leftCollisionRange;
    [SerializeField] RaycastParam upCollisionRange;
    [SerializeField] RaycastParam downCollisionRange;

    private bool collidesDown = false;
    private bool collidesLeft = false;
    private bool collidesUp = false;
    private bool collidesRight = false;
    #endregion

    #region Parameters GD
    [Header("Parameters")]
    [Tooltip("si actif, le joueur en se tournant gardera son momentum. Ex : le joueur est a sa vitesse maxiamale, il le sera encore en se tournant")] 
    [SerializeField] bool convertVelocityOnTurningAround;
    #endregion

    #region InitInputs
    void EnableInputAction(bool ShouldActivate)
    {
        foreach(InputAction action in _actions)
        {
            if(ShouldActivate) action.Enable();
            else action.Disable();
        }
    }
    private void OnEnable()
    {
        EnableInputAction(true);
        _actions[1].performed += Attack;
    }
    private void OnDisable()
    {
        EnableInputAction(false);
    }
    #endregion
    [System.Serializable]
    public struct RaycastParam
    {
        public Vector3 dimmensions;
        public Vector3 offset;

        public RaycastParam(float lengthx=1f, float lengthy=1f, float offsetx=1f,float offsety=1f)
        {
            this.dimmensions = new Vector3(lengthx, lengthy, 0);
            this.offset = new Vector3(offsetx, offsety, 0);
        }
    }
    void OnDrawGizmosSelected()
    {
        if (seeGizmos)
        {
            // Draws a 5 unit long red line in front of the object
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position + rightCollisionRange.offset, rightCollisionRange.dimmensions);
            Gizmos.DrawCube(transform.position + leftCollisionRange.offset, leftCollisionRange.dimmensions);
            Gizmos.DrawCube(transform.position + upCollisionRange.offset, upCollisionRange.dimmensions);
            Gizmos.DrawCube(transform.position + downCollisionRange.offset, downCollisionRange.dimmensions);
        }

    }
    void Awake()
    {
        //Setup Liste des inputs
        _IPplayercontrols = this.GetComponent<PlayerInput>().actions;
        _actions = new List<InputAction>();
        for (int i = 0; i < _IPplayercontrols.actionMaps[0].actions.Count; i++)
        {
            _actions.Add(_IPplayercontrols.actionMaps[0].actions[i]);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        castDirections();
        CalculateVerticalVelocity();
        CalculateHorizontalVelocity();


        transform.Translate(velocityDirection);
    }

    void CalculateHorizontalVelocity()
    {
        float InputDirection = _actions[0].ReadValue<Vector2>().x;
        // On check d'abord si le joueur est en train d'aller vers un mur/collision. Si c'est le cas, on l'arrête
        if ((InputDirection > 0 ||velocityDirection.x>0) && collidesRight || (InputDirection < 0 || velocityDirection.x<0) && collidesLeft)
        {
            velocityDirection.x = 0;
            return;
        }
        // Si le joueur bouge, on le bouge en conséquence
        if (InputDirection != 0)
        {
            #region KeepVelocityTurningAround
            // Si le joueur se tourne, on transfère le momentum
            if (convertVelocityOnTurningAround)
            {
                velocityDirection.x = Mathf.Abs(velocityDirection.x) * Mathf.Sign(InputDirection);
            }
            // Si le joueur se tourne, on reset le momentum
            else
            {
                if (Mathf.Sign(InputDirection) != Mathf.Sign(velocityDirection.x))
                {
                    velocityDirection.x = 0;
                }
            }
            #endregion

            velocityDirection.x += InputDirection * Time.deltaTime * moveSpeed / 10;
        }
        // Si non, on le fait décélérer
        else velocityDirection.x = Mathf.MoveTowards(velocityDirection.x, 0, friction * Time.deltaTime);

        //On clamp la movespeed lorsqu'il bouge
        velocityDirection.x = Mathf.Clamp(velocityDirection.x, -maxSpeed / 10, maxSpeed / 10);

    }
    void CalculateVerticalVelocity()
    {
        float inputDirection = _actions[0].ReadValue<Vector2>().y;

        if (collidesDown && velocityDirection.y < 0 || collidesDown && inputDirection <= 0) { velocityDirection.y = 0;  return; }
        if (inputDirection>0 && collidesDown && !collidesUp)
        {
            velocityDirection.y += jumpForce*Time.deltaTime;
        }
        velocityDirection.y -= gravityScale * Time.deltaTime;
        velocityDirection.y = Mathf.Clamp(velocityDirection.y, -maxFallSpeed, maxFallSpeed);

    }

    void castDirections()
    {
        //On cast sur les 4 directions et obtiens les bool de "est ce que une direction touche une surface"
        collidesDown = Physics.CheckBox(transform.position + downCollisionRange.offset, downCollisionRange.dimmensions/2);
        collidesLeft = Physics.CheckBox(transform.position + leftCollisionRange.offset, leftCollisionRange.dimmensions / 2);
        collidesUp = Physics.CheckBox(transform.position + upCollisionRange.offset, upCollisionRange.dimmensions / 2);
        collidesRight = Physics.CheckBox(transform.position + rightCollisionRange.offset, rightCollisionRange.dimmensions/2);
    }

    private void Attack(InputAction.CallbackContext context)
    {
        transform.DOShakeScale(1);
        transform.DOShakePosition(1);
    }


}