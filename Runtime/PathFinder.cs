using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace PathFinder
{

    public class PathFinder : MonoBehaviour
    {
        Grid grid;
        PathRequestManager requestManager;

        private void Awake()
        {
            grid = GetComponent<Grid>();
            requestManager = GetComponent<PathRequestManager>();
        }

        public void StartFindPath(Vector2 startPos, Vector2 endPos)
        {
            StartCoroutine(FindPath(startPos, endPos));
        }

        IEnumerator FindPath(Vector2 startPos, Vector2 endPos)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            bool passSuccess = false;
            Node startNode = grid.GetNodeFromWorldPoint(startPos);
            Node endNode = grid.GetNodeFromWorldPoint(endPos);
            //TODO create a way for the unit to go as close to the no walkable node as possilbe
            if (startNode.walkable && endNode.walkable)
            {
                Heap<Node> openQueue = new Heap<Node>(grid.MaxSize);
                HashSet<Node> closedSet = new HashSet<Node>();

                openQueue.Add(startNode);

                while (openQueue.Count > 0)
                {
                    Node currentNode = openQueue.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == endNode)
                    {
                        passSuccess = true;
                        break;
                    }

                    foreach (Node neighbour in grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.walkable || closedSet.Contains(neighbour))
                        {
                            continue;
                        }

                        int newCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                        if (newCost < neighbour.gCost || !openQueue.Contains(neighbour))
                        {
                            neighbour.gCost = newCost;
                            neighbour.hCost = GetDistance(neighbour, endNode);
                            neighbour.parent = currentNode;

                            if (!openQueue.Contains(neighbour))
                            {
                                openQueue.Add(neighbour);
                            }
                            else
                            {
                                openQueue.UpdateItem(neighbour);
                            }
                        }

                    }
                    closedSet.Add(currentNode);
                }
            }
            yield return null;
            sw.Stop();
            if (passSuccess)
            {
                UnityEngine.Debug.Log("Path found " + sw.ElapsedMilliseconds + " ms");
                var points = RetracePath(startNode, endNode);
                requestManager.FinishProcessingPath(points, true);
            }
            else
            {
                UnityEngine.Debug.Log("Path not found " + sw.ElapsedMilliseconds + " ms");
                //TODO improve API
                requestManager.FinishProcessingPath(new Vector2[0], false);
            }

        }

        Vector2[] RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;
            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            var waypoints = SimplifyPath(path);
            waypoints.Reverse();
            return waypoints.ToArray();
        }

        List<Vector2> SimplifyPath(List<Node> path)
        {
            List<Vector2> waypoints = new List<Vector2>();
            Vector2 directionOld = Vector2.zero;
            for (int i = 1; i < path.Count; i++)
            {
                Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
                if (directionNew != directionOld)
                {
                    waypoints.Add(path[i].position);
                }
                directionOld = directionNew;
            }
            return waypoints;
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            if (dstX > dstY)
            {
                return 14 * dstY + 10 * (dstX - dstY);
            }
            else
            {
                return 14 * dstX + 10 * (dstY - dstX);
            }
        }
    }

}