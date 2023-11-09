using Cinemachine.Utility;
using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 center;
    public bool FollowPlayers;

    void Start()
    {
        CameraTransition.Instance.cameraFollow = this;
    }
    void Update()
    {
        if (FollowPlayers)
        {
            foreach(var player in RoundManager.Instance.alivePlayers)
            {
                center += player._playerStateMachine.transform.position;
            }

            Camera Maincam = CameraTransition.Instance.MainCam;
            Camera cam = CameraTransition.Instance.TransitionCam;
            CameraParams Params = CameraTransition.Instance.cameraParams;
            Vector3 CamPos = cam.transform.position;

            Vector3 dir = (cam.transform.forward + cam.transform.rotation.eulerAngles).normalized;
            Vector3 toADD = (dir * Vector3.Distance(center, CamPos)).Abs();

            center /= RoundManager.Instance.alivePlayers.Count;
            center.z = CamPos.z;
            center.x = Mathf.Clamp(center.x, Maincam.transform.position.x-Params.cameraXmax/ 2, Maincam.transform.position.x + Params.cameraXmax/2);
            center.y =Mathf.Clamp(center.y, Maincam.transform.position.y -Params.cameraYmax/2, Maincam.transform.position.y +Params.cameraYmax/2);

            cam.transform.DOMove(center, CameraTransition.Instance.cameraParams.CameraFollowSmoothness);
        }
    }
}
