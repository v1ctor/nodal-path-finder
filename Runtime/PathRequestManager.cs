using System.Collections.Generic;
using System;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequests = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    PathFinder pathFinder;

    bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        pathFinder = GetComponent<PathFinder>();
    }

    public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> pathCallback)
    {
        PathRequest pathRequest = new PathRequest(pathStart, pathEnd, pathCallback);
        instance.pathRequests.Enqueue(pathRequest);
        instance.TryProcessNext();
    }

    public void FinishProcessingPath(Vector2[] points, bool result)
    {
        currentPathRequest.callback(points, result);
        isProcessingPath = false;
        TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!isProcessingPath && pathRequests.Count > 0)
        {
            currentPathRequest = pathRequests.Dequeue();
            isProcessingPath = true;
            pathFinder.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    struct PathRequest
    {
        public Vector2 pathStart;
        public Vector2 pathEnd;
        public Action<Vector2[], bool> callback;

        public PathRequest(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback)
        {
            this.pathStart = pathStart;
            this.pathEnd = pathEnd;
            this.callback = callback;
        }
    }
}
