using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGhost : Ghost
{
    public LayerMask Wall;
    private List<Vector2> availableTilesPosition;
    private RaycastHit2D hit;
    private int numberOfPos;
    private float distance;
    private float smallestDistance;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        availableTilesPosition = new List<Vector2>();
        ResetRedGhost();
    }

    void FixedUpdate()
    {
        currentPosition = this.rb.position;
        if (nextDirection != Vector2.zero)
        {

            hit = Physics2D.BoxCast(currentPosition, new Vector2(0.85f, 0.85f), 0f, nextDirection, 1.5f, Wall);
            if (hit.collider == null)
            {
                ChangeDirection();
            }
        }

        this.rb.MovePosition(currentPosition + currentDirection * speed * Time.fixedDeltaTime);
    }

    private void ChangeDirection()
    {
        currentDirection = nextDirection;
        nextDirection = Vector2.zero;
    }

    public void ResetRedGhost()
    {
        this.transform.position = spawnPosition;
        currentDirection = Vector2.left;
    }


    private void OnNodeLocation(Vector2 nodePosition)
    {
        smallestDistance = 100.0f;
        LookForAvailableTiles(nodePosition);
        foreach(Vector2 possibleDirection in availableTilesPosition)
        {
            distance = Vector2.Distance(nodePosition + possibleDirection, pacmanPosition.position);
            if(distance < smallestDistance && possibleDirection != -currentDirection)
            {
                nextDirection = possibleDirection;
                smallestDistance = distance;
            }

            
        }
        availableTilesPosition.Clear();
    }

    private void LookForAvailableTiles(Vector2 nodePosition)
    {
        hit = Physics2D.Raycast(nodePosition, Vector2.down, 1.0f, Wall);
        if (hit.collider == null)
        {
            availableTilesPosition.Add(Vector2.down);
        }

        hit = Physics2D.Raycast(nodePosition, Vector2.up, 1.0f, Wall);
        if (hit.collider == null)
        {
            availableTilesPosition.Add(Vector2.up);
        }

        hit = Physics2D.Raycast(nodePosition, Vector2.left, 1.0f, Wall);
        if (hit.collider == null)
        {
            availableTilesPosition.Add(Vector2.left);
        }

        hit = Physics2D.Raycast(nodePosition, Vector2.right, 1.0f, Wall);
        if (hit.collider == null)
        {
            availableTilesPosition.Add(Vector2.right);
        }



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Node"))
        {
            OnNodeLocation(collision.gameObject.transform.position);
        }
    }

}
