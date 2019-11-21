using UnityEngine;

public class Unit : PathFollower
{
    public Transform target;
    
    private void Start() {
        RequestPath(target.position);
    }
}
