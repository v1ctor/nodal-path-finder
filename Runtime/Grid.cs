using System.Collections.Generic;
using UnityEngine;

namespace PathFinder
{

    public class Grid : MonoBehaviour
    {
        public LayerMask unwalkableMask;
        public Vector2 gridSize;
        public float nodeRadius;
        public bool displayGrid;
        Node[,] grid;

        float nodeDiameter;
        int gridSizeX;
        int gridSizeY;

        // TODO this is required only because heap isn't dynamically allocated
        public int MaxSize
        {
            get
            {
                return gridSizeX * gridSizeY;
            }
        }

        private void Awake()
        {
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);
            CreateGrid();
        }

        void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];
            Vector3 bottomLeft = transform.position - Vector3.right * gridSize.x / 2f - Vector3.up * gridSize.y / 2f;
            for (int i = 0; i < gridSizeX; i++)
            {
                for (int j = 0; j < gridSizeY; j++)
                {
                    Vector3 worldPoint = bottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.up * (j * nodeDiameter + nodeRadius);
                    bool walkable = !Physics2D.OverlapBox(worldPoint, Vector2.one * nodeDiameter, 0, unwalkableMask);
                    grid[i, j] = new Node(walkable, worldPoint, i, j);
                }
            }
        }

        public Node GetNodeFromWorldPoint(Vector2 pos)
        {
            float percentX = (pos.x + gridSize.x / 2f) / gridSize.x;
            float percentY = (pos.y + gridSize.y / 2f) / gridSize.y;

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
            return grid[x, y];
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;
                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }
            return neighbours;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, gridSize.y, 1));
            if (grid != null && displayGrid)
            {
                foreach (Node node in grid)
                {
                    if (node.walkable)
                    {
                        Gizmos.color = Color.white;
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                    }
                    Gizmos.DrawCube(node.position, Vector3.one * (nodeDiameter * 0.9f));
                }
            }
        }

    }

}