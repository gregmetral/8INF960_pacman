using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGhost : Ghost
{
    new void Start()
    {
        base.Start();

        spawnPosition = new Vector2(0, 3.5f);

        ResetGhost();
    }

    public override void ResetGhost()
    {
        base.ResetGhost();
        ResetRedGhost();
    }

    public void ResetRedGhost()
    {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Node"))
        {
            if (!scatterMode)
            {
                OnNodeLocation(collision.gameObject.transform.position);
            }
            else
            {
                ScatterMode(collision.gameObject);
            }
        }
    }
}
