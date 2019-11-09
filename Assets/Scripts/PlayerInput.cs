using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Grid grid;

    private List<BreadCrumb> BreadCrumbs = new List<BreadCrumb>();

    // Update is called once per frame
    void Update()
    {
        //Pathfinding demo
        if (Input.GetMouseButtonDown(0))
        {
            //Convert mouse click point to grid coordinates
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Point dstPos = grid.WorldToGrid(worldPos);

            if (dstPos != null && dstPos.X > 0 && dstPos.Y > 0 && dstPos.X < grid.Width && dstPos.Y < grid.Height)
            {
                //Convert player point to grid coordinates
                Point playerPos = grid.WorldToGrid(transform.position);

                //Find path from player to clicked position
                BreadCrumbs = PathFinder.FindPath(grid, playerPos, dstPos);

                DrawPath(BreadCrumbs);
            }
        }
    }

    private void OnGUI()
    {
        if (Application.isEditor)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;
            foreach (var bc in BreadCrumbs)
            {
                var pos = grid.GridToWorld(bc.position);

                Handles.Label(pos, bc.cost.ToString(), style);
            }
        }
    }

    private void DrawPath(List<BreadCrumb> points)
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.positionCount = points.Count;  //Need a higher number than 2, or crashes out
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.startColor = Color.yellow;
        lr.endColor = Color.yellow;

        for (int i = 0; i < points.Count; i++)
        {
            lr.SetPosition(i, grid.GridToWorld(points[i].position));
        }
    }

}
