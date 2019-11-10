using UnityEngine;
using System.Collections.Generic;

public static class PathFinder
{
    public static List<BreadCrumb> FindPath(Grid world, Point start, Point end)
    {
        BreadCrumb bc = FindPathReversed(world, start, end);
        BreadCrumb[] temp = new BreadCrumb[256];
        List<BreadCrumb> breadCrumbs = new List<BreadCrumb>();
        while (bc != null)
        {
            breadCrumbs.Add(bc);
            bc = bc.next;
        }
        return breadCrumbs;
    }

    private static BreadCrumb FindPathReversed(Grid world, Point start, Point end)
    {
        MinHeap<BreadCrumb> openList = new MinHeap<BreadCrumb>(256);
        BreadCrumb[,] brWorld = new BreadCrumb[world.Right, world.Top];
        BreadCrumb node;
        Point tmp;

        BreadCrumb current = new BreadCrumb(start);
        current.cost = 0;

        BreadCrumb finish = new BreadCrumb(end);
        brWorld[current.position.X, current.position.Y] = current;
        openList.Add(current);

        while (openList.Count > 0)
        {
            //Find best item and switch it to the 'closedList'
            current = openList.ExtractFirst();
            current.onClosedList = true;

            //Find neighbours
            for (int i = 0; i < surrounding.Length; i++)
            {
                tmp = new Point(current.position.X + surrounding[i].X, current.position.Y + surrounding[i].Y);

                if (!world.ConnectionIsValid(current.position, tmp))
                    continue;

                //Check if we've already examined a neighbour, if not create a new node for it.
                if (brWorld[tmp.X, tmp.Y] == null)
                {
                    node = new BreadCrumb(tmp);
                    brWorld[tmp.X, tmp.Y] = node;
                }
                else
                {
                    node = brWorld[tmp.X, tmp.Y];
                }

                if (node.onClosedList) {
                    continue;
                }
                
                float diff = 1.0f;
                // Use diagonal, move cost is 0.7
                if (node.position.X != current.position.X && node.position.Y != current.position.Y) {
                    diff = 0.7f;
                }
                //TODO we can get cost from the direction. It can be precomputed once
                //FIXME drop of the decimal part of the float, can be a precision drop
                float distance = Mathf.Sqrt(Mathf.Pow(end.X - node.position.X, 2) + Mathf.Pow(end.Y - node.position.Y, 2));
                float cost = current.cost + diff + distance;

                if (cost < node.cost)
                {
                    node.cost = cost;
                    node.next = current;
                }

                //If the node wasn't on the openList yet, add it 
                if (!node.onOpenList)
                {
                    //Check to see if we're done
                    if (node.Equals(finish))
                    {
                        node.next = current;
                        return node;
                    }
                    node.onOpenList = true;
                    openList.Add(node);
                }
            }
        }
        return null; //no path found
    }

    //Neighbour options
    //Our diamond pattern offsets top/bottom/left/right by 2 instead of 1
    private static readonly Point[] surrounding = {
        new Point(0, 2), new Point(-2, 0), new Point(2, 0), new Point(0,-2),
        new Point(-1, 1), new Point(-1, -1), new Point(1, 1), new Point(1, -1)
    };
}

