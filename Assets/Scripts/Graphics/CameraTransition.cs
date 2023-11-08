using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class CameraTransition : MonoBehaviour
{
    public CameraParams cameraParams;
    public Camera MainCam;
    public Camera TransitionCam;
    public Material Mat;
    public RenderTexture renderTex;
    public Image Target;
    public CameraFollow cameraFollow;


    [HideInInspector]
    public Vector3 initPos , origin;
    [HideInInspector]
    public float initOrtho;
    [HideInInspector]
    public Sequence mySequence;

    public static CameraTransition Instance { get; private set; }


    void OnDrawGizmos()
    {
        Camera cam = Camera.main;
        Vector3 matrixpos = cam.transform.position;
        matrixpos += cam.transform.rotation.normalized * Vector3.forward * cameraParams.ZpourSeReperer;
        matrixpos.y -=cam.transform.position.y;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(matrixpos, cam.transform.rotation, cam.transform.lossyScale) ;
        Gizmos.matrix = rotationMatrix;
        // Green
        Gizmos.color = new UnityEngine.Color(0.0f, 1.0f, 0.0f);
        Vector2 center = new Vector3(cam.pixelWidth/2, cam.pixelHeight / 2);
        Vector2 dimmensions = new Vector3(cameraParams.cameraXmax, cameraParams.cameraYmax);
        Gizmos.DrawWireCube(Camera.main.ScreenToWorldPoint(center), dimmensions);

        float screenAspect = (float)Screen.width / (float)Screen.height;
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = screenAspect * camHalfHeight;
        float camWidth = 2.0f * camHalfWidth;
        float camHeight = camHalfHeight * 2;
        Vector2 range = new Vector2(camWidth+ cameraParams.cameraXmax, camHeight + cameraParams.cameraYmax);
        Gizmos.DrawWireCube(Camera.main.ScreenToWorldPoint(center), dimmensions+ range);    
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    public void FreezeIt()
    {
            Sprite sprite = Sprite.Create(ScreenCapture.CaptureScreenshotAsTexture(), new Rect(0, 0, renderTex.width, renderTex.height), new Vector2(0.5f, 0.5f));
            Target.sprite = sprite;
    }
    public Sequence UnfreezeIt()
    {
        origin = Target.rectTransform.anchoredPosition;
        Vector3 pos = Target.rectTransform.anchoredPosition;
        pos.y -= cameraParams.heighOfFall;
        mySequence = DOTween.Sequence();
        mySequence.Append(Target.rectTransform.DOJumpAnchorPos(pos, cameraParams.jumpForce, cameraParams.numberOfJumps, cameraParams.jumpDuration));
        return (mySequence);
    }
    public void ResetCams()
    {
        Target.rectTransform.anchoredPosition = origin;
        Target.sprite = null;
        copyCharacteristics(MainCam, TransitionCam);
    }


    public void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
        TransitionCam.aspect = MainCam.aspect;
        renderTex.width = Screen.width;
        renderTex.height = Screen.height;
    }

    void ChangedActiveScene(Scene PreviousScene, Scene NextScene)
    {
        print("changedScene");
        Camera[] cams = FindObjectsOfType<Camera>();
        foreach(Camera cam in cams)
        {
                if(!(cam == TransitionCam || cam == MainCam) && cam.tag!="EditorOnly")
                {
                    copyCharacteristics(cam, TransitionCam);
                    copyCharacteristics(cam, MainCam);

                    cam.enabled = false;
                    Destroy(cam.gameObject);
                }
        }
    }

    void copyCharacteristics(Camera origin, Camera cameraToModify)
    {
        initPos = origin.transform.position;
        initOrtho = origin.orthographicSize;

        cameraToModify.transform.position = origin.transform.position;
        cameraToModify.transform.position = origin.transform.position;
        cameraToModify.orthographicSize = initOrtho;

    }
}
