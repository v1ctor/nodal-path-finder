using UnityEngine;

public class NodeConnection
{
	public Node Parent;
	public Node Node;
	public bool Valid;

	public NodeConnection (Node parent, Node node, bool valid)
	{
		Valid = valid;
		Node = node;
		Parent = parent;

		if (Node != null && Node.BadNode)
			Valid = false;
		if (Parent != null && Parent.BadNode)
			Valid = false;
	}
}