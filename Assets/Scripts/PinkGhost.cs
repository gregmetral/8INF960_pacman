using UnityEngine;

public class PinkGhost : Ghost
{
    public Pacman pacman;
    private int targetInFrontOfPacman = 4;

    new void Start()
    {
        base.Start();

        spawnPosition = new Vector2(0, -2.5f);

        ResetGhost();
    }

    public override void ResetGhost()
    {
        base.ResetGhost();
        ResetPinkGhost();
    }

    public void ResetPinkGhost()
    {
        currentDirection = Vector2.left;
    }

    private void OnNodeLocation(Vector2 nodePosition)
    {
        smallestDistance = 100.0f;
        LookForAvailableTiles(nodePosition);
        foreach (Vector2 possibleDirection in availableTilesPosition)
        {
            Vector2 objective = ReadPacmanFuturePosition();
            distance = Vector2.Distance(nodePosition + possibleDirection, objective);
            if (distance < smallestDistance && possibleDirection != -currentDirection)
            {
                nextDirection = possibleDirection;
                smallestDistance = distance;
            }
        }
        availableTilesPosition.Clear();
    }

    private Vector2 ReadPacmanFuturePosition()
    {
        return (Vector2)pacmanPosition.position + targetInFrontOfPacman * pacman.currentDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Node"))
        {
            if (scatterMode)
            {
                ScatterMode(collision.gameObject);
            }
            else if (frightened)
            {
                FrightenedModeMove(collision.gameObject);
            }
            else
            {
                OnNodeLocation(collision.gameObject.transform.position);
            }
        }
    }
}
