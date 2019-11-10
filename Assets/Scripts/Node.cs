using UnityEngine;

public class Node 
{
    public bool walkable;
    public Vector2 position;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    // I believe it shouldn't be here
    public Node parent;

    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    public Node(bool walkable, Vector2 pos, int gridX, int gridY) {
        this.walkable = walkable;
        this.position = pos;
        this.gridX = gridX;
        this.gridY = gridY;
    }

}
