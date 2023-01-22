using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[ExecuteAlways]
public class GridShaderBinder : SingletonMonoBehaviour<GridShaderBinder>
{
	public Material gridMat;
	public Material testMat;
	public GridManager movementManager;
	public Color GridColor;
	public static List<GridHilight> gridHilights =new List<GridHilight>();
	public Vector2Int cursorPos = new Vector2Int(-1,-1);
	private void Start()
	{
		movementManager = GetComponent<GridManager>();
		//GridColor = new Color(0, 0, 0, 0);
	}

	// Update is called once per frame
	private void Update()
	{
		if (movementManager == null) movementManager = GetComponent<GridManager>();

		//GridCells.Clear();
		//GridCells.Add(new Vector2Int(0, 0), Color.red);
		//GridCells.Add(new Vector2Int(1, 1), Color.red);
		//GridCells.Add(new Vector2Int(2, 2), Color.red);
		Texture2D tex = new Texture2D(movementManager.gridSizeX, movementManager.gridSizeZ, TextureFormat.ARGB32, false);
		tex.filterMode = FilterMode.Point;
		List<byte> bytes = new List<byte>();
		for (int y = 0; y < movementManager.gridSizeZ; y++)
		{
			for (int x = 0; x < movementManager.gridSizeX; x++)
			{
				
				Color color;
				if(cursorPos == new Vector2Int(x, y))
				{
					bytes.Add((byte)(255));
					bytes.Add((byte)(255));
					bytes.Add((byte)(255));
					bytes.Add((byte)(255));
					
					continue;
                }
				GridHilight hilight= gridHilights.FirstOrDefault(s => s.points.Contains(new Vector2Int(x, y)));

				if (hilight!=null)
                {
				
					bytes.Add((byte)(hilight.color.a * 255));
					bytes.Add((byte)(hilight.color.r * 255));
					bytes.Add((byte)(hilight.color.g * 255));
					bytes.Add((byte)(hilight.color.b * 255));
					continue;
				}

               
					bytes.Add((byte)(GridColor.a * 255));
					bytes.Add((byte)(GridColor.r * 255));
					bytes.Add((byte)(GridColor.g * 255));
					bytes.Add((byte)(GridColor.b * 255));
				
			
			}
		}

		// Load data into the texture and upload it to the GPU.
		tex.LoadRawTextureData(bytes.ToArray());
		tex.Apply();
		gridMat.SetFloat("_Size", movementManager.gridSize);
		gridMat.SetVector("_Center", new Vector2(-movementManager.transform.position.x, -movementManager.transform.position.z));
		gridMat.SetVector("_Grid_Size", new Vector2(movementManager.gridSizeX, movementManager.gridSizeZ));
		gridMat.SetTexture("_GridInfo", tex);
		testMat.SetTexture("_BaseMap", tex);
		
	}
}