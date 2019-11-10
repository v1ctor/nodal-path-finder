using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO copypasta of a unit
public class Player : MonoBehaviour
{
    public float speed = 5f;
    private Vector2[] path;
    private int targetIndex;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // TODO not sure about requesting camera this way, might be not optimal
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PathRequestManager.RequestPath(transform.position, pos, OnPathFound);
        }
    }

    private void OnPathFound(Vector2[] points, bool sucess)
    {
        if (sucess)
        {
            StopCoroutine("FollowPass");
            path = points;
            targetIndex = 0;
            StartCoroutine("FollowPass");
        }
    }

    IEnumerator FollowPass()
    {
        if (path.Length > 0)
        {
            Vector3 currentPoint = path[0];
            while (true)
            {
                if (transform.position == currentPoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentPoint = path[targetIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, currentPoint, speed * Time.deltaTime);
                yield return null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);
                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
