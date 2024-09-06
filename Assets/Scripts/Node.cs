using UnityEngine;

public class Node : MonoBehaviour
{
    public enum GhostNodeType
    {
        Red,
        Pink,
        Blue,
        Orange,
        None
    }

    public GhostNodeType nodeType;
}