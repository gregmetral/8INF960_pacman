using UnityEngine;

public class Ghost : MonoBehaviour
{
    protected Vector2 spawnPosition = new Vector2(0, 3.5f);
    protected Rigidbody2D rb;
    protected Vector2 currentPosition;
    public Vector2 currentDirection { get; protected set;  }
    protected Vector2 nextDirection;
    public float speed;

    public Transform pacmanPosition;
}
