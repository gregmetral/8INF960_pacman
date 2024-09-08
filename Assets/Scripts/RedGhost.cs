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

    public override void OnNodeLocation(GameObject gameObject)
    {
        Vector2 nodePosition = gameObject.transform.position;
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
}
