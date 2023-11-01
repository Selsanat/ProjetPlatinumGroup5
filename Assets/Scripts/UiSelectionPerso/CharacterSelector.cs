using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PlayerInput = UnityEngine.InputSystem.PlayerInput;
public class CharacterSelector : MonoBehaviour
{
    UnityEngine.InputSystem.PlayerInput playerInputs;
    private Vector2 input;
    private int index = 0;
    private float PaddingLeft = 0;
    HorizontalLayoutGroup horizontalLayoutGroup;

    void Awake()
    {
        Canvas canvas = ManagerManager.Instance.selectionPersoCanvas;
        transform.SetParent(canvas.transform);
        playerInputs = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        horizontalLayoutGroup = GetComponentInParent<HorizontalLayoutGroup>();
    }

    void Start()
    {
        //ManagerManager.Instance.
        transform.GetComponentInChildren<Button>().Select();
        playerInputs.actions.actionMaps[1].actions[2].performed += ctx => SwipeRight();
        playerInputs.actions.actionMaps[1].actions[3].performed += ctx => SwipeLeft();
    }

    void Update()
    {
        input = playerInputs.actions.actionMaps[0].actions[0].ReadValue<Vector2>();
        horizontalLayoutGroup.padding.left = (int)PaddingLeft;
    }

    void SwipeRight()
    {
        print("right");
        if (index < horizontalLayoutGroup.transform.childCount - 1)
        {
            index++;
            DOTween.To(() => PaddingLeft, x => PaddingLeft = x, -200*index, 1);
        }
    }
    void SwipeLeft()
    {
        print("left");
        if (index > 0)
        {
            index--;
            DOTween.To(() => PaddingLeft, x => PaddingLeft = x, -200*index, 1);
        }
    }
}
