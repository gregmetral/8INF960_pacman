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
    protected bool isScattered;

    public bool scatterMode;
    public bool home;
    public bool frightened;

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

    public GameObject eyes;
    public GameObject body;
    public GameObject frightenedBody;

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
        isScattered = false;
        scatterMode = false;
        home = false;
        SetNormalMode();
    }

    protected void FixedUpdate()
    {
        if (home)
        {
            currentPosition = rb.position;
            currentDirection = nextDirection;
        }
        else
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
        if (LookForAvailableTile(nodePosition, Vector2.up) != Vector2.zero)
        {
            availableTilesPosition.Add(Vector2.up);
        }
        if (LookForAvailableTile(nodePosition, Vector2.down) != Vector2.zero)
        {
            availableTilesPosition.Add(Vector2.down);
        }
        if (LookForAvailableTile(nodePosition, Vector2.left) != Vector2.zero)
        {
            availableTilesPosition.Add(Vector2.left);
        }
        if (LookForAvailableTile(nodePosition, Vector2.right) != Vector2.zero)
        {
            availableTilesPosition.Add(Vector2.right);
        }
    }

    protected Vector2 LookForAvailableTile(Vector2 nodePosition, Vector2 direction, float d = 1.0f)
    {
        hit = Physics2D.Raycast(nodePosition, direction, d, Wall);
        if (hit.collider == null)
        {
            return direction;
        }
        return Vector2.zero;
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
                Vector2 isOk = LookForAvailableTile(node.transform.position, dirToCheck, result.Value);
                if (isOk != Vector2.zero)
                {
                    dir = dirToCheck;
                }
            }
        }

        return dir;
    }

    public void SetFrightenedMode()
    {
        frightened = true;
        body.SetActive(false);
        eyes.SetActive(false);
        frightenedBody.SetActive(true);
        speed = 2.0f;
    }

    public void SetNormalMode()
    {
        frightened = false;
        body.SetActive(true);
        eyes.SetActive(true);
        frightenedBody.SetActive(false);
        speed = 7.0f;
    }

    public void FrightenedModeMove(GameObject obj)
    {
        Vector2 nodePosition = obj.transform.position;

        Vector2[] dirToCheck = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        List<Vector2> availableDir = new List<Vector2>();

        smallestDistance = 100.0f;

        foreach (var d in dirToCheck)
        {
            if (d != -currentDirection)
            {
                Vector2 isOk = LookForAvailableTile(nodePosition, d, 1.5f);

                if (isOk != Vector2.zero)
                {
                    availableDir.Add(d);
                }
            }
        }

        int index = Random.Range(0, availableDir.Count);

        nextDirection = availableDir[index];
    }

    public void ToggleScatterMode()
    {
        scatterMode = !scatterMode;
    }

    public void LeaveGhostHouse()
    {
        home = false;
        transform.position = new Vector2(0, 3.5f);
        nextDirection = Vector2.left;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (home)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                if (currentDirection == Vector2.up)
                {
                    nextDirection = Vector2.down;
                }
                else if (currentDirection == Vector2.down)
                {
                    nextDirection = Vector2.up;
                }
                else if (currentDirection == Vector2.left)
                {
                    nextDirection = Vector2.right;
                }
                else if (currentDirection == Vector2.right)
                {
                    nextDirection = Vector2.left;
                }
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened)
            {
                ResetGhost();
                LeaveGhostHouse();
            }
            else
            {
                FindObjectOfType<GameManager>().OnPacmanDeath();
            }
        }

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

    public virtual void OnNodeLocation(GameObject gameObject)
    {
    }
}