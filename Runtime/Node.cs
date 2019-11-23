using UnityEngine;

namespace PathFinder
{


    public class Node : IHeapItem<Node>
    {
        public bool walkable;
        public Vector2 position;
        public int gridX;
        public int gridY;

        public int gCost;
        public int hCost;
        // I believe it shouldn't be here
        public Node parent;
        private int heapIndex;

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }
        public Node(bool walkable, Vector2 pos, int gridX, int gridY)
        {
            this.walkable = walkable;
            this.position = pos;
            this.gridX = gridX;
            this.gridY = gridY;
        }

        public int CompareTo(Node other)
        {
            int compare = fCost.CompareTo(other.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(other.hCost);
            }
            return -compare;
        }
    }
}