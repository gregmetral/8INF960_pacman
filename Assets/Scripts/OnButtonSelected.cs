using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnButtonSelected : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    public bool shouldBeSelected = false;
    public RectTransform pacman;

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
        RectTransform rect = GetComponent<RectTransform>();
        pacman.position = new Vector3(
            rect.position.x - rect.rect.width / 2,
            rect.position.y,
            pacman.position.z
        );
    }
}
