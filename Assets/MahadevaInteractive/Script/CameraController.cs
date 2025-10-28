using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // Player

    [Header("Camera Follow")]
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Position offset
    public float followLag = 5f; // Higher = snappier, Lower = smoother

    [Header("Lock Options")]
    public bool followX = true;
    public bool followY = true;

    private void LateUpdate()
    {
        if (target == null) return;

        // Base desired position (no rotation)
        Vector3 desiredPosition = transform.position;

        if (followX)
            desiredPosition.x = target.position.x + offset.x;

        if (followY)
            desiredPosition.y = target.position.y + offset.y;

        desiredPosition.z = offset.z;

        // Smoothly move camera toward target
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followLag * Time.deltaTime);

        // Always stay upright — no tilt or rotation from player
        transform.rotation = Quaternion.identity;
    }
}
