using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnButtonSelected : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    public bool shouldBeSelected = false;
    public MainMenuPacman pacman;

    private void Awake()
    {
        if (shouldBeSelected)
        {
            GetComponent<Button>().Select();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Button>().Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        MovePacman();
    }

    private void MovePacman()
    {
        pacman.FaceButton(GetComponent<RectTransform>());
    }

    public void FeedPacman()
    {
        pacman.EatButton();
    }
}
