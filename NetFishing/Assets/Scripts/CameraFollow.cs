using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.N3DS;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // The player's transform
    public Vector3 offset = new Vector3(0f, -5f, -15f); // The camera's initial offset
    public float smoothSpeed = 0.125f; // Smoothing factor for camera movement
    public float rotationSpeed = 100f; // Speed at which the camera orbits around the player

    public float currentAngleY = 0f; // Horizontal rotation around the player
    public float currentAngleX = 30f; // Vertical rotation (tilt) angle

    void FixedUpdate()
    {
        // Read Circle Pad Pro input (ranges from -1 to 1)
        Vector2 circlePadPro = UnityEngine.N3DS.GamePad.CirclePadPro;
        if (UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.Start))
        {
            circlePadPro = UnityEngine.N3DS.GamePad.CirclePad;
        }

        // Adjust the rotation angles based on input
        currentAngleY += circlePadPro.x * rotationSpeed * Time.deltaTime; // Horizontal rotation
        currentAngleX -= circlePadPro.y * rotationSpeed * Time.deltaTime; // Vertical rotation (inverted Y axis)

        // Clamp the vertical angle to avoid flipping the camera
        currentAngleX = Mathf.Clamp(currentAngleX, 10f, 80f);

        // Calculate the new camera position
        Quaternion rotation = Quaternion.Euler(currentAngleX, currentAngleY, 0);
        Vector3 desiredPosition = player.position + rotation * offset;

        // Smoothly interpolate to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;

        // Ensure the camera looks at the player
        transform.LookAt(player.position + Vector3.up * 1.5f); // Adjust look-at height for better focus
    }
}
