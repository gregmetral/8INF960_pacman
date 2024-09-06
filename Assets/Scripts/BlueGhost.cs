using System.Collections.Generic;
using UnityEngine;

public class BlueGhost : Ghost
{
    public LayerMask Wall;
    private List<Vector2> availableTilesPosition;
    private RaycastHit2D hit;
    private Vector2 spawnPositionBlue = new Vector2(-4.5f, 0.5f);

    private bool home = false;
    private bool frightened = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        availableTilesPosition = new List<Vector2>();
        ResetBlueGhost();
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

    public void ResetBlueGhost()
    {
        this.transform.position = spawnPositionBlue;
        currentDirection = Vector2.left;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (collision.gameObject.layer == LayerMask.NameToLayer("Node"))
        {
            OnNodeLocation(collision.gameObject.transform.position);
        }
    }

    private void OnNodeLocation(Vector2 nodePosition)
    {
        LookForAvailableTiles(nodePosition);

        int index = Random.Range(0, availableTilesPosition.Count);

        if (this.home == false && this.frightened == false){
            if (availableTilesPosition[index] == -this.currentDirection && availableTilesPosition.Count > 1){

                index++;

                if(index >= availableTilesPosition.Count){
                    index = 0;
                }
            }

            this.nextDirection = availableTilesPosition[index];

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

}
