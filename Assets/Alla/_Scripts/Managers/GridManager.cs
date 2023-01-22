using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GridManager : SingletonMonoBehaviour<GridManager>
{
	public GridGraph grid;
	public int gridSizeX;
	public int gridSizeZ;

	[Range(0.1f,3f)]
	public float gridSize = 1f;
	private List<Node> lastPath = new List<Node>();
	public Vector3 _end;
	public Vector3 _start;
    private void Awake()
    {
        base.Awake();
		grid = new GridGraph(gridSizeX, gridSizeZ);
		grid.Walls = new List<GridGraph.GridType>();
		grid.Forests = new List<Vector2Int>();
	}
	public bool CanMove(Vector2Int cell)
	{
		if (Obstical.IsObstacl(cell)) return false;
		if (grid.Walls.FirstOrDefault((e) => { return e.pos == cell; }) == null) return false;

		return true;
	}
	private void Start()
	{

		//foreach (Obstical obs in GetComponentsInChildren<Obstical>())
		//{
		//	graph.Walls.Add(new Vector2((int)obs.transform.position.x, (int) obs.transform.position.z));
		//}
	}

	public bool PathBetween(Vector2Int start, Vector2Int end, out List<Node> path)
	{
		List<Node> tempPath = AStar.Search(grid, grid.Grid[start.x, start.y], grid.Grid[end.x, end.y]);
	
		_end = GridToWorld(end);
		_start = GridToWorld(tempPath[0].Position);

		lastPath = tempPath;
		if (tempPath.Count > 0)
		{
			path = tempPath;
			return true;
		}
		path = null;
		return false;
	}

	public  Vector3 GridToWorld(Vector2Int pos)
	{
		return new Vector3(transform.position.x + (pos.x * gridSize + gridSize / 2),
			transform.position.y,
			transform.position.z + (pos.y * gridSize + gridSize / 2));
	}

	public  bool WorldToGrid(Vector3 pos,out Vector2Int gridPos)
	{
		gridPos = new Vector2Int((int)((pos.x - transform.position.x) / gridSize),
			(int)((pos.z - transform.position.z) / gridSize));
        if (gridPos.x<gridSizeX && gridPos.x>=0 && gridPos.y < gridSizeZ && gridPos.y >= 0)
		{
			return true;
		}
		return false;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeZ; y++)
			{
				Gizmos.DrawSphere(GridToWorld(new Vector2Int(x, y)) - new Vector3(gridSize / 2, 0, gridSize / 2), gridSize / 10);
			}
		}
		Gizmos.color = Color.red;
		if (lastPath.Count <= 0) return;
		foreach (Node node in lastPath)
		{
			Gizmos.DrawSphere(GridToWorld(node.Position), gridSize / 10);
		}
		Gizmos.color = Color.black;
		Gizmos.DrawSphere(_end, gridSize / 10);
		Gizmos.color = Color.white;
		Gizmos.DrawSphere(_start, gridSize / 10);
	}
}