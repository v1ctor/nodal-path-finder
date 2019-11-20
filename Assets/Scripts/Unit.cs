using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    const float pathUpdateMoveThreshold = .5f;
    const float minPassUpdateTime = .2f;

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

    IEnumerator UpdatePath() {
        if (Time.timeSinceLevelLoad < .3f) {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        
        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;
        while (true) {
            yield return new WaitForSeconds(minPassUpdateTime);
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                targetPosOld = target.position;
            }
        }
    }

    IEnumerator FollowPass()
    {
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);
        while (followingPath)
        {
            while (path.turnBoundaries[pathIndex].HasCrossedLine(transform.position)) {
                if (pathIndex == path.finishLineIndex) {
                    followingPath = false;
                    break;
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
