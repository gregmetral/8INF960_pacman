using UnityEngine;

public class BlueGhost : Ghost
{
    new void Start()
    {
        base.Start();

        spawnPosition = new Vector2(-4.5f, 0.5f);

        ResetGhost();
    }

    public override void ResetGhost()
    {
        base.ResetGhost();
        ResetBlueGhost();
    }

    public void ResetBlueGhost()
    {
        currentDirection = Vector2.left;
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
}
