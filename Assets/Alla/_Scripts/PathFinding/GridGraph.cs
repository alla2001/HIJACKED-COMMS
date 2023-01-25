using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GridGraph
{
    public int Width;
    public int Height;

    public Node[,] Grid;
    public List<GridType> Walls = new List<GridType>();
    public List<Vector2Int> Forests;

    public class GridType
    {
       public Vector2Int pos;

    }
   
    public void AddObstacles(List<Vector2Int> points)
    {
        foreach (Vector2Int point in points)
        {
            Walls.Add(new GridType { pos = point});
        }
    }
    public GridGraph(int w, int h)
    {
        Width = w;
        Height = h;

        Grid = new Node[w, h];

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Grid[x, y] = new Node(x, y);
            }
        }
    }

    /// <summary>
    /// Checks whether the neighbouring Node is within the grid bounds or not
    /// </summary>
    public bool InBounds(Vector2 v)
    {
        if (v.x >= 0 && v.x < this.Width &&
            v.y >= 0 && v.y < this.Height)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Checks whether the neighbouring Node is a wall or not
    /// </summary>
    public bool Passable(Vector2Int id)
    {
        if (Walls.FirstOrDefault((e) => { return e.pos == id; })!=null) return false;
        else return true;
    }

    /// <summary>
    /// Returns a List of neighbouring Nodes
    /// </summary>
    /// 
    public List<Vector2Int> directions = new List<Vector2Int>()
        {
            new Vector2Int( -1, 0 ), // left
            //new Vector2Int(-1, 1 ),  // top-left, comment it out for 4-direction movement
            new Vector2Int( 0, 1 ),  // top
            //new Vector2Int( 1, 1 ),  // top-right, comment it out for 4-direction movement
            new Vector2Int( 1, 0 ),  // right
            //new Vector2Int( 1, -1 ), // bottom-right, comment it out for 4-direction movement
            new Vector2Int( 0, -1 ), // bottom
            //new Vector2Int( -1, -1 ) // bottom-left, comment it out for 4-direction movement
        };
    public List<Node> Neighbours(Node n)
    {
        List<Node> results = new List<Node>();

        

        foreach (Vector2Int v in directions)
        {
            Vector2Int newVector = v + n.Position;
            if (InBounds(newVector) && Passable(newVector))
            {
                results.Add(Grid[(int)newVector.x, (int)newVector.y]);
            }
        }

        return results;
    }

    public int Cost(Node b)
    {
        // If Node 'b' is a Forest return 2, otherwise 1
        if (Forests.Contains(b.Position)) return 2;
        else return 1;
    }
}
