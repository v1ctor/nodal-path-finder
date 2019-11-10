using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    Grid grid;
    public Transform seeker;
    public Transform target;

    private void Update() {
        FindPath(seeker.position, target.position);
    }

    private void Awake() {
        grid = GetComponent<Grid>();
    }

    List<Node> FindPath(Vector2 startPos, Vector2 endPos) {
        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node endNode = grid.GetNodeFromWorldPoint(endPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0) {
            //TODO replace with heap
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                //Can use hCost to optimise a little bit but it seems that isn't going to be used in the heap approach
                if (openSet[i].fCost < currentNode.fCost) {
                    currentNode = openSet[i];
                }
            }

            //TODO list remove isn't optimal
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode) {
                // TODO return path
                return RetracePath(startNode, endNode);
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                    continue;
                }

                int newCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newCost < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newCost;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                    }
                }

            }
            closedSet.Add(currentNode);
        }
        return new List<Node>();
    }

    List<Node> RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        grid.path = path;
        return path;
    }

    int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY) {
            return 14 * dstY + 10 * (dstX - dstY);
        } else {
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}
