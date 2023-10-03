using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Rendering;
using TMPro;

//rajouté un struct qui a les raycast param 
//pour la détéction de la coll, tracé un trait et voir s'il est entre un coté 
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
    [Tooltip("Force de saut")][SerializeField] float MinJump = 1;
    [Tooltip("Fall speed")] [SerializeField] float gravityScale = 5;
    [Tooltip("Fall speed maximale")] [SerializeField] float maxFallSpeed = 5;
    [Tooltip("Vitesse maximale horizontale du joueur")] [SerializeField] float maxSpeed = 5;
    [Tooltip("Friction avec le sol après un mouvement. Low friction -> ice effect")] [SerializeField] float friction = 5;
    private Vector3 velocityDirection;
    #endregion

    #region RaycastDetection
    [Header("Collisions hitboxes (see gizmo)")]
    [SerializeField] bool seeGizmos = true;
    [SerializeField] RaycastParam CollisionRange;

    public bool collidesDown = false;
    public bool collidesLeft = false;
    public bool collidesUp = false;
    public bool collidesRight = false;
    #endregion

    #region Parameters GD
    [Header("Parameters")]
    [Tooltip("si actif, le joueur en se tournant gardera son momentum. Ex : le joueur est a sa vitesse maxiamale, il le sera encore en se tournant")] 
    [SerializeField] float keepVelocityOnTurnAround;
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
    public class RaycastParam
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
            Gizmos.DrawCube(transform.position + CollisionRange.offset, CollisionRange.dimmensions);
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
        
        CalculateVerticalVelocity();
        CalculateHorizontalVelocity();
        castDirections();
        applyVelocity();
    }

    void CalculateHorizontalVelocity()
    {
        float InputDirection = _actions[0].ReadValue<Vector2>().x;
        // On check d'abord si le joueur est en train d'aller vers un mur/collision. Si c'est le cas, on l'arrête
        // Si le joueur bouge, on le bouge en conséquence
        if (InputDirection != 0)
        {
            #region KeepVelocityTurningAround
            // Si le joueur se tourne, on transfère une partie du momentum

            if (Mathf.Sign(InputDirection) != Mathf.Sign(velocityDirection.x))
            {
                velocityDirection.x = (Mathf.Abs(velocityDirection.x)* Mathf.Sign(InputDirection)) *(keepVelocityOnTurnAround / 100);
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

        // Si le joueur collides vers le bas (donc au sol) et qu'il est sous gravité, on l'arrête et return
        // On l'arrête et return  également si il ne saute pas et est au sol pour éviter les calculs inutiles
        velocityDirection.y -= gravityScale * Time.deltaTime;

        if (inputDirection > 0)
        {
            if(collidesDown && !collidesUp)
            {
                velocityDirection.y += jumpForce * Time.deltaTime;
            }
        }
        else
        {
            velocityDirection.y = Mathf.Clamp(velocityDirection.y, -maxFallSpeed, jumpForce/MinJump);
            return;
        }

        velocityDirection.y = Mathf.Clamp(velocityDirection.y, -maxFallSpeed, maxFallSpeed);

    }
    void applyVelocity()
    {
        Vector2 inputDirection = _actions[0].ReadValue<Vector2>();
        if (collidesDown && velocityDirection.y < 0 || collidesUp && velocityDirection.y > 0 || collidesDown && inputDirection.y <= 0) { velocityDirection.y = 0; }
        if ((inputDirection.x > 0 || velocityDirection.x > 0) && collidesRight || (inputDirection.x < 0 || velocityDirection.x < 0) && collidesLeft) { velocityDirection.x = 0; }
        transform.Translate(velocityDirection);
    }
    void ResetCollideCheck()
    {
        collidesDown = false;
        collidesRight = false;
        collidesLeft = false;
        collidesUp = false;
    }

    void castDirections()
    {
        //On cast sur les 4 directions et obtiens les bool de "est ce que une direction touche une surface"
        Vector3 tempNextPlayerPos = transform.position + velocityDirection + CollisionRange.offset;

        // Pour chaque axe, teste si le joueur va se retrouver dans un mur après le déplacement de la prochaine frame
        // Si le joueur sera dans un mur après s'être déplacer, cela veut donc dire qu'il collides avec un objet dans la direction dans laquelle il veut aller
        Collider[] tempRaycastHitsPlayer = Physics.OverlapBox(tempNextPlayerPos, CollisionRange.dimmensions / 2, Quaternion.identity);
/*        if(tempRaycastHitsPlayer.Length==0) {
            ResetCollideCheck();
            return; 
        }*/
        bool tempup = false;
        bool tempdown = false;
        bool templeft = false;
        bool tempright = false;
        foreach (Collider hit in tempRaycastHitsPlayer)
        {
            Vector3 tempClosest = hit.ClosestPointOnBounds(tempNextPlayerPos);
            print(tempClosest.x + "Collision, Point :" + (tempNextPlayerPos.x - CollisionRange.dimmensions.x / 2));
            tempdown = tempdown || (velocityDirection.y<0 &&  tempNextPlayerPos.y-CollisionRange.dimmensions.y/2 <=tempClosest.y) ;
            tempup = tempup || (velocityDirection.y > 0 && tempNextPlayerPos.y + CollisionRange.dimmensions.y / 2 >= tempClosest.y);

            templeft = templeft ||( tempNextPlayerPos.x - CollisionRange.dimmensions.x / 2 >= tempClosest.x);
            tempright = tempright ||(tempNextPlayerPos.x + CollisionRange.dimmensions.x / 2 <= tempClosest.x);
        }
        collidesDown = tempdown;
        collidesLeft = templeft;
        collidesRight = tempright;
        collidesUp = tempup;
        
    }

    private void Attack(InputAction.CallbackContext context)
    {
        transform.DOShakeScale(1);
        transform.DOShakePosition(1);
    }


}