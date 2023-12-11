using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class mapSelector : MonoBehaviour
{
    int index = 0;
    HorizontalLayoutGroup horizontalLayoutGroup;
    float PaddingLeft = 0;
    InputSystemUIInputModule playerInputs;
    public Image droit, gauche;
    private void Start()
    {
        horizontalLayoutGroup = GetComponentInChildren<HorizontalLayoutGroup>();
        playerInputs = FindObjectOfType<InputSystemUIInputModule>();
        playerInputs.move.ToInputAction().performed += ctx => Swipe();
    }

    private void Update()
    {
        horizontalLayoutGroup.padding.left = (int)PaddingLeft;
        LayoutRebuilder.ForceRebuildLayoutImmediate(horizontalLayoutGroup.GetComponent<RectTransform>());
    }

    void Swipe()
    {
        if(GameStateMachine.Instance.CurrentState != GameStateMachine.Instance.MapSelectionState)
        {
            return;
        }
        if(playerInputs.move.action.ReadValue<Vector2>().x > 0)
        {
            SwipeRight();
        }
        if (playerInputs.move.action.ReadValue<Vector2>().x < 0)
        {
            SwipeLeft();
        }
    }
    void SwipeRight()
    {
        if (index < horizontalLayoutGroup.transform.childCount - 1)
        {
            click(droit);
            index++;
            DOTween.To(() => PaddingLeft, x => PaddingLeft = x, (-100 - horizontalLayoutGroup.spacing) * index, 1);
        }
    }
    void SwipeLeft()
    {
        click(gauche);
        if (index > 0)
        {
            index--;
            DOTween.To(() => PaddingLeft, x => PaddingLeft = x, (-100-horizontalLayoutGroup.spacing) * index, 1);
        }
    }

    void click(Image im)
    {
        SoundManager.instance.PlayRandomClip("Click");
        im.DOColor(Color.gray, 0.25f).OnComplete(() => im.DOColor(Color.white, 0.25f));
    }
}
