using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{

    private Rigidbody2D rb;
    public float speed;
    private Vector2 currentDirection;
    private Vector2 nextDirection;
    private Vector2 spawnPosition = new Vector2(0f,-8.5f);
    private Vector2 currentPosition;
    public LayerMask Wall;
    private RaycastHit2D hit;

    public Transform render;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetPacman();
    }

    public void ResetPacman()
    {
        //reset la position de pacman
        this.transform.position = spawnPosition;
        currentDirection = Vector2.zero;
        nextDirection = Vector2.zero;
    }

    void Update()
    {
        //Input
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            nextDirection = Vector2.up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            nextDirection = Vector2.down;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            nextDirection = Vector2.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextDirection = Vector2.right;
        }
    }


    void FixedUpdate()
    {
        currentPosition = this.rb.position;

        //check pour tourner pacman 
        if (nextDirection != Vector2.zero)
        {
            
            hit = Physics2D.BoxCast(currentPosition, new Vector2(0.85f,0.85f), 0f, nextDirection, 1.5f, Wall);
            if(hit.collider == null)
            {
                ChangeDirection();
            }
        }

        //dépalcement
        this.rb.MovePosition(currentPosition + currentDirection * speed * Time.fixedDeltaTime);
    }

    private void ChangeDirection()
    {
        if (nextDirection == Vector2.up)
        {
            render.rotation = Quaternion.Euler(0, 0, 90);
        } else if (nextDirection == Vector2.down)
        {
            render.rotation = Quaternion.Euler(0, 0, -90);
        } else if (nextDirection == Vector2.right)
        {
            render.rotation = Quaternion.Euler(0, 0, 0);
        } else if (nextDirection == Vector2.left)
        {
            render.rotation = Quaternion.Euler(0, 0, 180);
        }

        currentDirection = nextDirection;
        nextDirection = Vector2.zero;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HERE");
    }
}
