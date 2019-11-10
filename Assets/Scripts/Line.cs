using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{
    const float verticalLineGradient = 1e5f;
    float gradient;
    float intercept;
    float perpendicular;
    Vector2 pointOnLine1;
    Vector2 pointOnLine2;

    bool approachSide;

    public Line(Vector2 pointOnLine, Vector2 pointPerpendicular) {
        float dx = pointOnLine.x - pointPerpendicular.x;
        float dy = pointOnLine.x - pointPerpendicular.y;
        
        if (dx == 0) {
            perpendicular = verticalLineGradient;
        } else {
            perpendicular = dy / dx;
        }
        if (perpendicular == 0) {
            gradient = verticalLineGradient;
        } else {
            gradient = -1 / perpendicular;
        }

        intercept = pointOnLine.y - gradient * pointOnLine.x;
        pointOnLine1 = pointOnLine;
        pointOnLine2 = pointOnLine + new Vector2(1, gradient);

        approachSide = false;
        approachSide = GetSide(pointPerpendicular);
    }

    bool GetSide(Vector2 p) {
        return (p.x - pointOnLine1.x) * (pointOnLine2.y - pointOnLine1.y) > (p.y - pointOnLine1.y) * (pointOnLine2.x - pointOnLine1.x);
    }

    public bool HasCrossedLine(Vector2 p) {
        return GetSide(p) != approachSide;
    }

    public void DrawWithGizmos(float length) {
        Vector3 lineDir = new Vector3(1, gradient).normalized;
        Vector3 lineCenter = new Vector3(pointOnLine1.x, pointOnLine1.y);
        Gizmos.DrawLine(lineCenter - lineDir * length / 2f, lineCenter + lineDir * length / 2f);
    }
}
