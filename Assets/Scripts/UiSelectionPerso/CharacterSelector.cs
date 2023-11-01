using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    PlayerInput playerInput;
    private Vector2 input;

    void Awake()
    {
        Canvas canvas = ManagerManager.Instance.selectionPersoCanvas;
        transform.SetParent(canvas.transform);
        PlayerInput playerInputs = GetComponent<PlayerInput>();

    }

    void Start()
    {
        transform.GetComponentInChildren<Button>().Select();
    }

    void Update()
    {
    }
}
