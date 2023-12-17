using Cinemachine.Utility;
using DG.Tweening;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class CameraFollow : MonoBehaviour
{
    private Vector3 center;
    public bool finale;
    public bool FollowPlayers;
    public LensDistortion LD;
    float height, width;
    void Start()
    {
        CameraTransition.Instance.cameraFollow = this;
    }
    void Update()
    {
        finale = RoundManager.Instance.alivePlayers.Count == 2 && RoundManager.Instance.players.Count!=2;
        center = Vector3.zero;

            foreach(var player in RoundManager.Instance.alivePlayers)
            {
                if(player._playerStateMachine != null)
                center += player._playerStateMachine.transform.position;
            }

            Camera Maincam = CameraTransition.Instance.MainCam;
            Camera cam = CameraTransition.Instance.TransitionCam;
            CameraParams Params = CameraTransition.Instance.cameraParams;
            Vector3 CamPos = cam.transform.position;

            Vector3 dir = (cam.transform.forward + cam.transform.rotation.eulerAngles).normalized;
            Vector3 toADD = (dir * Vector3.Distance(center, CamPos)).Abs();
            Volume vol = RoundManager.Instance.Volume;
            vol.profile.TryGet<LensDistortion>(out LensDistortion LD);
        if (finale)
        {
            if (FollowPlayers)
            {
                CameraTransition.Instance.TransitionCam.orthographicSize = 12;
                cam.WorldToScreenPoint(center);
                height = 2f * cam.orthographicSize;
                width = height * cam.aspect;
                float cx = (center.x / width) * 0.2f + 0.5f;
                float cy = (center.y / height) * 0.2f + 0.5f;
                DOTween.To(() => LD.intensity.value, x => LD.intensity.value = x, 0.5f, 1);
                DOTween.To(() => LD.center.value, x => LD.center.value = x, new Vector2(cx, cy), 1);
            }
        }
        else
        {
            LD.intensity.value = 0;
            LD.center.value = new Vector2(0.5f, 0.5f);
            if (FollowPlayers)
            {
                CameraTransition.Instance.TransitionCam.orthographicSize = 10;
                center /= RoundManager.Instance.alivePlayers.Count;
                center.z = CamPos.z;
                center.x = Mathf.Clamp(center.x, Maincam.transform.position.x - Params.cameraXmax / 2, Maincam.transform.position.x + Params.cameraXmax / 2);
                center.y = Mathf.Clamp(center.y, Maincam.transform.position.y - Params.cameraYmax / 2, Maincam.transform.position.y + Params.cameraYmax / 2);
                cam.transform.DOMove(center, CameraTransition.Instance.cameraParams.CameraFollowSmoothness);
            }
        }
    }
}
