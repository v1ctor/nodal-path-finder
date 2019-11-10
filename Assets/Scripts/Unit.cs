using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float turnSpeed = 3f;
    public float turnDst = 5f;
    Path path;

    void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    private void OnPathFound(Vector2[] waypoints, bool sucess)
    {
        if (sucess)
        {
            path = new Path(waypoints, transform.position, turnDst);
            StopCoroutine("FollowPass");
            StartCoroutine("FollowPass");
        }
    }

    IEnumerator FollowPass()
    {
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);
        while (followingPath)
        {
            if (path.turnBoundaries[pathIndex].HasCrossedLine(transform.position)) {
                if (pathIndex == path.finishLineIndex) {
                    followingPath = false;
                } else {
                    pathIndex++;
                }
            }
            if (followingPath) {
                Vector2 position = transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }
}
