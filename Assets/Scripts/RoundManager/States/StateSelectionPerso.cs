public class StateSelectionPerso : GameStateTemplate
{
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(GameStateTemplate gameStateTemplate)
    {
        StateMachine.HideAllMenusExceptThis(ui);
    }

    protected override void OnStateUpdate()
    {

    }
}