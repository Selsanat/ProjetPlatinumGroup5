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
        print("swipe");
        print((playerInputs.move.action.ReadValue<Vector2>()));
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
            index++;
            DOTween.To(() => PaddingLeft, x => PaddingLeft = x, -100 * index, 1);
        }
    }
    void SwipeLeft()
    {
        if (index > 0)
        {
            index--;
            DOTween.To(() => PaddingLeft, x => PaddingLeft = x, -100 * index, 1);
        }
    }
}
