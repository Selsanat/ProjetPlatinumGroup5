using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Image = UnityEngine.UI.Image;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.Linq;

public class CameraTransition : MonoBehaviour
{
    public CameraParams cameraParams;
    public Camera MainCam;
    public Camera TransitionCam;
    public Material Mat;
    public RenderTexture renderTex;
    public Image Target;
    public CameraFollow cameraFollow;
    private Vector2 res;


    [HideInInspector]
    public Vector3 initPos , origin;
    [HideInInspector]
    public float initOrtho;
    [HideInInspector]
    public Sequence mySequence;
    ChromaticAberration CA;
    Volume vol;

    public static CameraTransition Instance { get; private set; }


    public void CameraShake()
    {
        Volume vol = RoundManager.Instance.Volume;
        vol.profile.TryGet<ChromaticAberration>(out ChromaticAberration CA);
        float init = CA.intensity.value;
        DOTween.To(() => CA.intensity.value, x => CA.intensity.value = x, 100, cameraParams.TimeToShakePlayerDeath/3);
        Camera.main.DOShakePosition(cameraParams.TimeToShakePlayerDeath, cameraParams.ShakeForcePlayerDeath, cameraParams.vibratoShakeDeath, cameraParams.RandomnessShakeDeath, cameraParams.ShouldFadeShakeDeath).OnComplete(() =>
        {
            DOTween.To(() => CA.intensity.value, x => CA.intensity.value = x, init + 0.3f, 1);
        });
    }
    public void CameraRotation()
    {
        TransitionCam.DOShakeRotation(cameraParams.RotatePlayerWinTime, cameraParams.ForceRotateWin, cameraParams.VibratoRotateWin, cameraParams.RandomnesRotateDeath, cameraParams.ShouldFadeRotateWin);
    }
    
    void OnDrawGizmos()
    {
        Camera cam;
        cam = TransitionCam;

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
        Vector2 range = new Vector2(camWidth*2+ cameraParams.cameraXmax, camHeight + cameraParams.cameraYmax);
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

    IEnumerator RecordFrame()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("RecordFrame");
        Sprite sprite = Sprite.Create(ScreenCapture.CaptureScreenshotAsTexture(), new Rect(0, 0, Screen.width, Screen.height), new Vector2(0.5f, 0.5f));
        Target.sprite = sprite;
    }

    public void FreezeIt()
    {
        StartCoroutine(RecordFrame());
    }
    public Sequence UnfreezeIt()
    {
        origin = Target.rectTransform.anchoredPosition;
        Vector3 pos = Target.rectTransform.anchoredPosition;
        pos.y -= cameraParams.heighOfFall;
        mySequence = DOTween.Sequence();
        mySequence.Append(Target.rectTransform.DOJumpAnchorPos(pos, cameraParams.jumpForce, cameraParams.numberOfJumps, cameraParams.jumpDuration).OnComplete( ()=>
        {
            if(TransitionCam.targetTexture != null)
            {
                TransitionCam.targetTexture.Release();
            }
            RenderTexture rd = new RenderTexture(Screen.width, Screen.height, 0);
            TransitionCam.targetTexture = rd;
            Material mat = new Material(Shader.Find("Sprites/Default"));//"Sprites/Default
            mat.mainTexture = rd;
            Target.material = mat;
        }));
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
        RenderSettings.skybox.SetFloat("_Rotation", 0);
        DOTween.To(() => RenderSettings.skybox.GetFloat("_Rotation"), x => RenderSettings.skybox.SetFloat("_Rotation", x), 360, 240).SetLoops(-1);
    }
    public void Update()
    {
        if (res.x != Screen.width || res.y != Screen.height)
        {
            res = new Vector2(Screen.width, Screen.height);
            RenderTexture rd = new RenderTexture(Screen.width, Screen.height, 0);
            TransitionCam.targetTexture = rd;
            Material mat = new Material(Shader.Find("Sprites/Default"));//"Sprites/Default
            mat.mainTexture = rd;
            Target.material = mat;
        }
    }

    void ChangedActiveScene(Scene PreviousScene, Scene NextScene)
    {
        List<Camera> cams = FindObjectsOfType<Camera>().ToList();
        cams.RemoveAll(x => x.tag == "CamDecors");
        foreach (Camera cam in cams)
        {
            print(cam);
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
