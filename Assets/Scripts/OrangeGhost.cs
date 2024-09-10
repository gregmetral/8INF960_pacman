using UnityEngine;

public class OrangeGhost : Ghost
{
    new void Start()
    {
        base.Start();
        spawnPosition = new Vector2(2f, 0.5f);
        ResetGhost();
    }

    public override void ResetGhost()
    {
        base.ResetGhost();
        ResetOrangeGhost();
    }

    public void ResetOrangeGhost()
    {
        nextDirection = Vector2.down;
        home = true;
    }

    public override void OnNodeLocation(GameObject node)
    {
        Vector2 nodePosition = node.transform.position;

        // Orange gets near Pacman but when at less than 8 tiles, runs away to its corner
        float distanceToPacman = Vector2.Distance(nodePosition, pacmanPosition.position);
        if (distanceToPacman > 8)
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
        else
        {
            ScatterMode(node);
        }

    }
}