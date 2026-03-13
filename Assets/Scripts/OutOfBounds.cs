using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    [Header("Boundary Settings")]
    [SerializeField]
    private float padding = 0.5f;

    private Camera mainCamera;
    private float minX, maxX, minY, maxY;

    private void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No main camera found in the scene!");
            return;
        }

        // Calculate the screen boundaries in world space
        CalculateBoundaries();
    }

    private void CalculateBoundaries()
    {
        // Get the camera's orthographic size (half of the camera view height)
        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Calculate boundaries based on camera position
        Vector3 cameraPos = mainCamera.transform.position;

        minX = cameraPos.x - cameraWidth + padding;
        maxX = cameraPos.x + cameraWidth - padding;
        minY = cameraPos.y - cameraHeight + padding;
        maxY = cameraPos.y + cameraHeight - padding;

        Debug.Log("Boundaries set - X: " + minX + " to " + maxX + ", Y: " + minY + " to " + maxY);
    }

    private void LateUpdate()
    {
        // Clamp the player position within the boundaries
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(transform.position.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(transform.position.y, minY, maxY);

        // Apply the clamped position
        transform.position = clampedPosition;
    }
}
