using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using PlayerInput = UnityEngine.InputSystem.PlayerInput;
public class CharacterSelector : MonoBehaviour
{
    UnityEngine.InputSystem.PlayerInput playerInputs;
    public int index = 0;
    private float PaddingLeft = 0;
    private HorizontalLayoutGroup horizontalLayoutGroup;
    private Toggle toggle;
    private MultiplayerEventSystem multiplayerEventSystem;
    [SerializeField] private Button buttonsImages;
    void Awake()
    {
        Canvas canvas = ManagerManager.Instance.selectionPersoCanvas;
        transform.SetParent(canvas.transform);
        playerInputs = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        horizontalLayoutGroup = GetComponentInChildren<HorizontalLayoutGroup>();
        toggle = GetComponentInChildren<Toggle>();
        multiplayerEventSystem = GetComponent<MultiplayerEventSystem>();
    }

    void Start()
    {
        ManagerManager manager = ManagerManager.Instance;
        manager.characterSelector.Add(this);
        playerInputs.actions.actionMaps[1].actions[2].started += ctx => SwipeRight();
        playerInputs.actions.actionMaps[1].actions[3].started += ctx => SwipeLeft();
        toggle.onValueChanged.AddListener((value) =>
        {
            
            if (value)
            {
                ManagerManager.Instance.Players[playerInputs.devices[0]] = (RoundManager.Team)index;
                toggle.navigation = new Navigation() { mode = Navigation.Mode.None };
            }
            else
            {
                ManagerManager.Instance.Players.Remove(playerInputs.devices[0]);
                toggle.navigation = new Navigation() { mode = Navigation.Mode.Vertical };
            }
            UpdateCard();

            ManagerManager.Instance.ReadyToFight.isOn = CanStart();
        });

        manager.ReadyToFight.isOn = CanStart();

        if (manager.Players.Values.Contains((RoundManager.Team)0)) buttonsImages.transform.GetChild(0).GetComponent<Image>().color =
            new Color(0.5f, 0.5f, 0.5f);
    }

    void Update()
    {
        playerInputs.actions.actionMaps[1].actions[4].performed += ctx =>
        {
            if (ManagerManager.Instance.ReadyToFight.isOn)
            {
                ManagerManager.Instance.ReadyToFight.isOn = false;
                GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.MapSelectionState);
            }
        };
        horizontalLayoutGroup.padding.left = (int)PaddingLeft;
        LayoutRebuilder.ForceRebuildLayoutImmediate(horizontalLayoutGroup.GetComponent<RectTransform>());
        if (!toggle.isOn)
        {
            toggle.interactable = !ManagerManager.Instance.Players.ContainsValue((RoundManager.Team)index);
        }
    }
    void SwipeRight()
    {
        if (index < horizontalLayoutGroup.transform.childCount - 1 && multiplayerEventSystem.currentSelectedGameObject != toggle.gameObject)
        {
            index++;
            DOTween.To(() => PaddingLeft, x => PaddingLeft = x, -200*index, 1);
        }
    }
    void SwipeLeft()
    {
        if (index > 0 && multiplayerEventSystem.currentSelectedGameObject != toggle.gameObject)
        {
            index--;
            DOTween.To(() => PaddingLeft, x => PaddingLeft = x, -200*index, 1);
        }
    }

    void UpdateCard()
    {
        ManagerManager manager = ManagerManager.Instance;
        foreach (CharacterSelector selec in manager.characterSelector)
        {
            if (selec != this)
            {
                for (int i = 0; i < buttonsImages.transform.childCount; i++)
                {
                    if (manager.Players.ContainsValue((RoundManager.Team)i) && (selec.index!=i || selec.index == i && !selec.toggle.isOn))
                    {
                        selec.buttonsImages.interactable = false;
                        selec.buttonsImages.transform.GetChild(i).GetComponent<Image>().color =
                        new Color(0.5f, 0.5f, 0.5f);
                    }
                    else
                    {
                        selec.buttonsImages.interactable = true;
                        selec.buttonsImages.transform.GetChild(i).GetComponent<Image>().color = new Color(1, 1, 1);
                    }
                }
            }
        }
    }

    bool CanStart()
    {
        foreach (var player in ManagerManager.Instance.characterSelector)
        {
            if (!player.toggle.isOn)
            {
                return false;
            }
        }
        return true;
    }
}
