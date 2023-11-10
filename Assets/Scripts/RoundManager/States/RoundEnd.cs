using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundEnd : GameStateTemplate
{
    private Camera cam;
    protected override void OnStateInit()
    {
    }

    protected override void OnStateEnter(GameStateTemplate gameStateTemplate)
    {
        CameraTransition.Instance.cameraFollow.FollowPlayers = false;
        StateMachine.HideAllMenusExceptThis(ui);
        cam = CameraTransition.Instance.TransitionCam;
        cam.DOOrthoSize(5,1);
        StateMachine.StartCoroutine(NextRound());
    }

    protected override void OnStateUpdate()
    {
        if (cam != null)
        {
            Vector3 playerPos = RoundManager.Instance.alivePlayers[0]._playerStateMachine.gameObject.transform.position;
            playerPos.z = -10;
            CameraTransition.Instance.TransitionCam.transform.DOMove(playerPos, 0.1f);
        }
    }

    IEnumerator NextRound()
    {
        yield return new WaitForSeconds(ManagerManager.Instance.gameParams.TransiTimeAfterRound);
        foreach (RoundManager.Player player in RoundManager.Instance.players)
        {
            if (player._points >= ManagerManager.Instance.gameParams.PointsToWin)
            {
                SceneManager.LoadScene("Victory");
                yield break;
            }
        }
        StateMachine.StartCoroutine(RoundManager.Instance.NewRound());
    }
}