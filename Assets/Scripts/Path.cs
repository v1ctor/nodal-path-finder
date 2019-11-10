using UnityEngine;

public class Path 
{
    public readonly Vector2[] lookPoints;
    public readonly Line[] turnBoundaries;
    public readonly int finishLineIndex;

    public Path(Vector2[] lookPoints, Vector2 startPoint, float turnDst)
    {
        this.lookPoints = lookPoints;
        this.turnBoundaries = new Line[lookPoints.Length];
        this.finishLineIndex = lookPoints.Length - 1;

        Vector2 prevPoint = startPoint;
        for (int i = 0; i < lookPoints.Length; i++) {
            Vector2 currentPoint = lookPoints[i];
            Vector2 dirToCurPoint = (currentPoint - prevPoint).normalized;
            Vector2 turnBoundaryPoint = (i == finishLineIndex) ? currentPoint : currentPoint - dirToCurPoint * turnDst;
            turnBoundaries[i] = new Line(turnBoundaryPoint, prevPoint - dirToCurPoint * turnDst);
            prevPoint = turnBoundaryPoint;
        }
    }

    public void DrawWithGizmos() {
        Gizmos.color = Color.black;
        foreach (Vector2 p in lookPoints) {
            Gizmos.DrawCube(p, Vector3.one);
        }

        Gizmos.color = Color.white;
        foreach(Line l in turnBoundaries) {
            l.DrawWithGizmos(10);
        }
    }
}
