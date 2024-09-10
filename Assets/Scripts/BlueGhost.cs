using UnityEngine;

public class BlueGhost : Ghost
{
    new void Start()
    {
        base.Start();
        spawnPosition = new Vector2(-2f, 0.5f);
        ResetGhost();
    }

    public override void ResetGhost()
    {
        base.ResetGhost();
        ResetBlueGhost();
    }

    public void ResetBlueGhost()
    {
        nextDirection = Vector2.down;
        home = true;
    }

    public override void OnNodeLocation(GameObject gameObject)
    {
        Vector2 nodePosition = gameObject.transform.position;
        LookForAvailableTiles(nodePosition);

        // Blue selects a random direction
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
}
