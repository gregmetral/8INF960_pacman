using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPacman : MonoBehaviour
{
    public bool shouldEat = false;
    public float moveSpeed = 5;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (shouldEat)
        {
            rect.position = new Vector3(
                rect.position.x + moveSpeed,
                rect.position.y,
                rect.position.z
            );
        }
    }

    public void FaceButton(RectTransform button)
    {
        if (!shouldEat)
        {
            rect.position = new Vector3(
                button.position.x - button.rect.width / 2,
                button.position.y,
                rect.position.z
            );
        }
    }

    public void EatButton()
    {
        shouldEat = true;
    }
}
