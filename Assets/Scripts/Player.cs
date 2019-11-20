using UnityEngine;

public class Player : PathFollower
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // TODO not sure about requesting camera this way, might be not optimal
            var targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RequestPath(targetPosition);
        }
    }
}
