using System.Collections;
using UnityEngine;

namespace PathFinder
{

    public abstract class PathFollower : MonoBehaviour
    {
        public float speed = 5f;
        Vector2[] path;
        int targetIndex;
        protected Vector3 direction;

        protected void RequestPath(Vector2 target)
        {
            PathRequestManager.RequestPath(transform.position, target, OnPathFound);
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
            if (path.Length == 0)
            {
                yield return null;
            }
            Vector3 currentPoint = path[0];
            direction = currentPoint - transform.position;
            direction.Normalize();
            while (true)
            {
                if (transform.position == currentPoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        direction = Vector3.zero;
                        yield break;
                    }
                    currentPoint = path[targetIndex];
                    direction = currentPoint - transform.position;
                    direction.Normalize();
                }
                transform.position = Vector3.MoveTowards(transform.position, currentPoint, speed * Time.deltaTime);
                yield return null;
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
}