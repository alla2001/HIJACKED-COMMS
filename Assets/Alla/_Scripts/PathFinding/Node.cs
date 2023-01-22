using System;
using UnityEngine;

public class Node : IComparable<Node>
{
    public Vector2Int Position;
    public float Priority;

    public Node(int x, int y)
    {
        Position = new Vector2Int(x, y);
    }
    public Node(Vector2Int vec)
    {
        Position = new Vector2Int(vec.x, vec.y);
    }
    public int CompareTo(Node other)
    {
        if (this.Priority < other.Priority) return -1;
        else if (this.Priority > other.Priority) return 1;
        else return 0;
    }
}
