using UnityEngine;

public class OrangeGhost : Ghost
{
    //new void Start()
    //{
    //    base.Start();
    //    spawnPosition = new Vector2(2f, 0.5f);
    //    ResetGhost();
    //}

    public override void ResetGhost()
    {
        base.ResetGhost();
        ResetOrageGhost();
    }

    public void ResetOrageGhost()
    {
        currentDirection = Vector2.down;
    }

    private void OnNodeLocation(GameObject node)
    {
        Vector2 nodePosition = node.transform.position;

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
                OnNodeLocation(collision.gameObject);
            }
        }
    }
}