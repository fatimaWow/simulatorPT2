using UnityEngine;

public class StatsMenuScript : MonoBehaviour
{
    public GameObject gameMenu;
    public Transform head;

    public float spawnDistance = 1.5f;
    public float heightOffset = -0.2f;

    public float horizontalOffset = 0.3f; // 👈 NEW (move left/right)

    public float smoothSpeed = 8f;

    void LateUpdate()
    {
        if (!gameMenu.activeSelf) return;

        // Flatten forward direction (ignore vertical tilt)
        Vector3 forward = head.forward;
        forward.y = 0;
        forward.Normalize();

        // Get right direction (also flattened)
        Vector3 right = head.right;
        right.y = 0;
        right.Normalize();

        // Build final target position
        Vector3 targetPosition =
            head.position
            + forward * spawnDistance
            + right * horizontalOffset   // 👈 shifts left/right
            + Vector3.up * heightOffset;

        // Smooth movement
        gameMenu.transform.position = Vector3.Lerp(
            gameMenu.transform.position,
            targetPosition,
            Time.deltaTime * smoothSpeed
        );

        // Face the player
        Vector3 lookTarget = new Vector3(
            head.position.x,
            gameMenu.transform.position.y,
            head.position.z
        );

        gameMenu.transform.LookAt(lookTarget);
        gameMenu.transform.forward *= -1;
    }
}