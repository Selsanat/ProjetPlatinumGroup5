using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateSelectionPerso : GameStateTemplate
{
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(GameStateTemplate gameStateTemplate)
    {
        StateMachine.HideAllMenusExceptThis(ui);
        foreach(CharacterSelector player in ManagerManager.Instance.characterSelector)
        {
            GameObject.Destroy(player.gameObject);
        }
        ManagerManager.Instance.characterSelector.Clear();

/*        foreach(PlayerInput pI in InputsManager.Instance.GetComponents<PlayerInput>())
        {
            GameObject.Destroy(pI);
        }*/
    }

    protected override void OnStateUpdate()
    {

    }
}