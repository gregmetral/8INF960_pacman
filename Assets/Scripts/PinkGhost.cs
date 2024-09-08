using UnityEngine;

public class PinkGhost : Ghost
{
    public Pacman pacman;
    private int targetInFrontOfPacman = 4;

    new void Start()
    {
        base.Start();

        spawnPosition = new Vector2(0, -0.5f);

        ResetGhost();
    }

    public override void ResetGhost()
    {
        base.ResetGhost();
        ResetPinkGhost();
    }

    public void ResetPinkGhost()
    {
        nextDirection = Vector2.up;
        home = true;
    }

    public override void OnNodeLocation(GameObject gameObject)
    {
        Vector2 nodePosition = gameObject.transform.position;
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
}
