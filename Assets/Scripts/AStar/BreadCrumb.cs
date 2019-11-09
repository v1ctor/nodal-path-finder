using System;

public class BreadCrumb : IComparable<BreadCrumb>
{
    public readonly Point position;
    public BreadCrumb prev;
    public BreadCrumb next;
    public int cost = int.MaxValue;
    public bool onClosedList;
    public bool onOpenList;

    public BreadCrumb(Point position)
    {
        this.position = position;
    }

    //Overrides default Equals so we check on position instead of object memory location
    public override bool Equals(object obj)
    {
        return (obj is BreadCrumb) && ((BreadCrumb)obj).position.X == position.X && ((BreadCrumb)obj).position.Y == position.Y;
    }

    //Faster Equals for if we know something is a BreadCrumb
    public bool Equals(BreadCrumb breadcrumb)
    {
        return breadcrumb.position.X == position.X && breadcrumb.position.Y == position.Y;
    }

    public override int GetHashCode()
    {
        return position.GetHashCode();
    }

    public int CompareTo(BreadCrumb other)
    {
        return cost.CompareTo(other.cost);
    }
}

