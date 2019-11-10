using UnityEngine;

public class Node 
{
    public bool walkable;
    public Vector2 position;

    public Node(bool walkable, Vector2 pos) {
        this.walkable = walkable;
        this.position = pos;
    }

}
