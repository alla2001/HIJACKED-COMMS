using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class GridIgnore : MonoBehaviour
{
    public BoxCollider collider;
    
    Vector3 bottomLeft;
    Vector3 topRight;
    public GridManager gridManager;
    GridHilight gh;
    public List<Vector2Int> points = new List<Vector2Int>();

    public static List<GridIgnore> gridIgnores = new List<GridIgnore>();
    // Start is called before the first frame update
    void Start()
    {
        gridIgnores.Add(this);
        GridShaderBinder.gridHilights.Clear();
         points.Clear();

        GridShaderBinder.gridHilights.Remove(gh);
        bottomLeft = new Vector3(collider.bounds.center.x - collider.bounds.extents.x, collider.center.y, collider.bounds.center.z - collider.bounds.extents.z);
        topRight = new Vector3(collider.bounds.center.x + collider.bounds.extents.x, collider.center.y, collider.bounds.center.z + collider.bounds.extents.z);
        Vector2Int blgrid;
        Vector2Int trgrid;
        gridManager.WorldToGrid(bottomLeft, out blgrid);
        gridManager.WorldToGrid(topRight, out trgrid);
        int ydiff = trgrid.y - blgrid.y + 1;
        int xdiff = trgrid.x - blgrid.x + 1;



        for (int x = 0; x < xdiff; x++)
        {
            for (int y = 0; y < ydiff; y++)
            {
                points.Add( new Vector2Int(blgrid.x + x, blgrid.y + y) );
            }

        }

        //GridManager.instance.grid.Walls.AddRange(points);


        gh = new GridHilight { color = new Color(0, 0, 0, 0), points = points };
        GridShaderBinder.gridHilights.Add(gh);
    }
    /* collider.bounds 
     Center : center of the collider in word space (not the same as transform.position)
     Extens : the size of the collider in X Y Z

     */
    // Update is called once per frame
    void Update()
    {
        //points.Clear();
        
        //GridShaderBinder.gridHilights.Remove(gh);
        //bottomLeft = new Vector3(collider.bounds.center.x - collider.bounds.extents.x, collider.center.y, collider.bounds.center.z - collider.bounds.extents.z);
        //topRight = new Vector3(collider.bounds.center.x + collider.bounds.extents.x, collider.center.y, collider.bounds.center.z + collider.bounds.extents.z);
        //Vector2Int blgrid;
        //Vector2Int trgrid;
        //gridManager.WorldToGrid(bottomLeft, out blgrid);
        //gridManager.WorldToGrid(topRight, out trgrid);
        //int ydiff = trgrid.y - blgrid.y+1;
        //int xdiff = trgrid.x - blgrid.x + 1;


       
        //for (int x = 0; x < xdiff; x++)
        //{
        //    for (int y = 0; y < ydiff; y++)
        //    {
        //        points.Add(new GridGraph.GridType {pos= new Vector2Int( blgrid.x + x, blgrid.y + y )});
        //    }

        //}
        
        //    //GridManager.instance.grid.Walls.AddRange(points);


        ////gh = new GridHilight { color = new Color(0, 0, 0, 0), points = points };
        //GridShaderBinder.gridHilights.Add(gh);

    }
    private void OnDestroy()
    {
        gridIgnores.Remove(this);
        GridShaderBinder.gridHilights.Remove(gh);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(bottomLeft, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(topRight, 0.2f);
        Gizmos.color = new Color(1,1,0,0.2f);
        Gizmos.DrawCube(collider.bounds.center, collider.bounds.size);
        Gizmos.color = new Color(1, 1, 0, 1);
        Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
    }
}
