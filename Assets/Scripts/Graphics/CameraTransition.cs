using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class CameraTransition : MonoBehaviour
{
    public Camera MainCam;
    public Camera TransitionCam;

    public Material Mat;
    public RenderTexture renderTex;
    public Image Target;
    public Vector3 initPos;
    public Vector3 origin;
    public float initOrtho;
    public Sequence mySequence;

    public static CameraTransition Instance { get; private set; }

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
        pos.y -= 500;
        mySequence = DOTween.Sequence();
        mySequence.Append(Target.rectTransform.DOJumpAnchorPos(pos, 500, 1, 0.1f));
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
