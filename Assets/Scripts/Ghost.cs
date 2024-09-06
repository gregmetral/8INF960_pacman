using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    protected Vector2 spawnPosition;
    protected Rigidbody2D rb;
    protected Vector2 currentPosition;
    public Vector2 currentDirection { get; protected set;  }
    protected Vector2 nextDirection;
    public float speed;


    public Transform pacmanPosition;
    protected bool isScattered = false;

    public bool scatterMode = false;
    public bool home = false;
    public bool frightened = false;

    public Transform Nodes;
    protected List<Node> scatterNodes;
    protected int indexScatterNode = 0;

    public Node.GhostNodeType ghostType;

    public LayerMask Wall;
    public LayerMask NodeMask;
    protected List<Vector2> availableTilesPosition;
    protected RaycastHit2D hit;
    protected int numberOfPos;
    protected float distance;
    protected float smallestDistance;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        availableTilesPosition = new List<Vector2>();

        scatterNodes = new List<Node>();
        foreach (Transform node in Nodes)
        {
            Node n = node.GetComponent<Node>();
            if (n.nodeType == ghostType)
            {
                scatterNodes.Add(n);
            }
        }
    }

    public virtual void ResetGhost()
    {
        transform.position = spawnPosition;
    }

    protected void FixedUpdate()
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

        rb.MovePosition(currentPosition + currentDirection * (speed * Time.fixedDeltaTime));
    }

    protected void ChangeDirection()
    {
        currentDirection = nextDirection;
        nextDirection = Vector2.zero;
    }

    protected void LookForAvailableTiles(Vector2 nodePosition)
    {
        LookForAvailableTile(nodePosition, Vector2.up);
        LookForAvailableTile(nodePosition, Vector2.down);
        LookForAvailableTile(nodePosition, Vector2.left);
        LookForAvailableTile(nodePosition, Vector2.right);
    }

    protected void LookForAvailableTile(Vector2 nodePosition, Vector2 direction, float d = 1.0f)
    {
        hit = Physics2D.Raycast(nodePosition, direction, d, Wall);
        if (hit.collider == null)
        {
            availableTilesPosition.Add(direction);
        }
    }

    protected void ScatterMode(GameObject obj)
    {
        Node node = obj.GetComponent<Node>();

        if (node)
        {
            if (node.nodeType == ghostType)
            {
                isScattered = true;
            }
            else
            {
                isScattered = false;
            }

            if (!isScattered)
            {
                Transform target = scatterNodes[indexScatterNode].transform;

                LookForAvailableTiles(obj.transform.position);

                smallestDistance = float.MaxValue;

                foreach (Vector2 dir in availableTilesPosition)
                {
                    Vector2 targetPosition = target.position;
                    Vector2 nodePosition = obj.transform.position;
                    Vector2 nextPosition = nodePosition + dir;

                    float d = Vector2.Distance(targetPosition, nextPosition);

                    if (d < smallestDistance && dir != -currentDirection)
                    {
                        smallestDistance = d;
                        nextDirection = dir;
                    }
                }

                availableTilesPosition.Clear();
            }
            else
            {
                Vector2[] dirToCheck = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

                foreach (Vector2 d in dirToCheck)
                {
                    Vector2 dir = CanGoScatterNode(d, obj.transform);

                    if (dir != Vector2.zero)
                    {
                        nextDirection = d;
                        return;
                    }
                }
            }
        }
    }

    protected KeyValuePair<Vector2, float> CheckAllScatterNode(Transform node, Vector2 direction)
    {
        Vector2 dir = Vector2.zero;

        RaycastHit2D[] hits = Physics2D.RaycastAll(node.position, direction, 20.0f, NodeMask);

        float distanceToTarget = float.MaxValue;

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider != null)
            {
                Node n = hits[i].collider.GetComponent<Node>();
                if (n && n.nodeType == ghostType && n.transform.position != node.position)
                {
                    float d = Vector2.Distance(n.transform.position, node.position);
                    if (d < distanceToTarget)
                    {
                        dir = direction;
                        distanceToTarget = d;
                    }
                }
            }
        }

        return new KeyValuePair<Vector2, float>(dir, distanceToTarget);
    }

    protected Vector2 CanGoScatterNode(Vector2 dirToCheck, Transform node)
    {
        Vector2 dir = Vector2.zero;
        if (dirToCheck != -currentDirection)
        {
            var result = CheckAllScatterNode(node, dirToCheck);

            if (result.Key != Vector2.zero)
            {
                LookForAvailableTile(node.transform.position, dirToCheck, result.Value);
                if (availableTilesPosition.Count > 0)
                {
                    dir = dirToCheck;
                    availableTilesPosition.Clear();
                }
            }
        }

        return dir;
    }
}