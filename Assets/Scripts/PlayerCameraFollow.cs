using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    private Vector3 playerPosition;
    public Vector3 offset = new Vector3(-10f, 5f, -10f);
    public float smoothSpeed = 0.125f;

    public bool useConstraints = true;
    public Vector2 xConstraint = new Vector2(-100f, 100f);
    public Vector2 yConstraint = new Vector2(-10f, 20f);

    public void SetPlayerPosition(Vector3 pos) => playerPosition = pos;

    void LateUpdate()
    {
        if (playerPosition == Vector3.zero) return;

        Vector3 desired = playerPosition + offset;
        if (useConstraints)
        {
            desired.x = Mathf.Clamp(desired.x, xConstraint.x, xConstraint.y);
            desired.y = Mathf.Clamp(desired.y, yConstraint.x, yConstraint.y);
        }

        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed);
    }
}
