using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEyes : MonoBehaviour
{
    private SpriteRenderer eyes;
    public Sprite leftEye;
    public Sprite rightEye;
    public Sprite upEye;
    public Sprite downEye;

    private Ghost ghost;

    void Start()
    {
        eyes = GetComponent<SpriteRenderer>();
        ghost = GetComponentInParent<Ghost>();
    }

    private void FixedUpdate()
    {
        if (ghost.currentDirection == Vector2.left)
        {
            eyes.sprite = leftEye;
        }
        else if (ghost.currentDirection == Vector2.right)
        {
            eyes.sprite = rightEye;
        }
        else if (ghost.currentDirection == Vector2.up)
        {
            eyes.sprite = upEye;
        }
        else if (ghost.currentDirection == Vector2.down)
        {
            eyes.sprite = downEye;
        }
    }
}
