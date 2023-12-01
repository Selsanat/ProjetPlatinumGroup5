using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MenuState : GameStateTemplate
{
    HorizontalLayoutGroup horizontalLayoutGroup => ManagerManager.Instance.horizontalLayoutGroup;
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(GameStateTemplate gameStateTemplate)
    {
        InputsManager.Instance.playerInputs.Clear();
        Volume vol = ManagerManager.Instance.Volume;
        DepthOfField dof;

        //CameraTransition.Instance.Target.transform.position = new Vector3(0, 0, 0);

        vol.profile.TryGet<DepthOfField>(out dof);
        horizontalLayoutGroup.padding.top = -400;
        horizontalLayoutGroup.spacing = 0;
        dof.focalLength.value = 0;
        foreach (Transform child in horizontalLayoutGroup.transform)
        {
            child.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        foreach(GameObject cadrant in RoundManager.Instance.cadrants)
        {
            cadrant.gameObject.SetActive(false);
            foreach (var score in RoundManager.Instance.scores)
            {
                score.text = "";
            }
        }

        if (ui == null) ui = StateMachine.Menus[StateMachine.AllStates.ToList().IndexOf(this)].menuObject;
        StateMachine.HideAllMenusExceptThis(ui);
        if(StateMachine.PreviousState == StateMachine.endRound)
        {
            ManagerManager.Instance.Players.Clear();
            RoundManager.Instance.players.Clear();
            RoundManager.Instance.alivePlayers.Clear();
            CameraTransition.Instance.UnfreezeIt();
            PlayerInputManager.instance.GetComponentInChildren<PlayerInput>();
        }
    }

    protected override void OnStateUpdate()
    {

    }
}
