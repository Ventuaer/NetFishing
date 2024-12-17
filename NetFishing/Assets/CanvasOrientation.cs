using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasOrientation : MonoBehaviour
{
    public Transform cameraTransform; // Drag your main camera here in the Inspector

    void Update()
    {
        // Get the position of the camera, but lock its Y coordinate to match the canvas's position
        Vector3 targetPosition = new Vector3(cameraTransform.position.x, transform.position.y, cameraTransform.position.z);

        // Make the canvas look at the camera's target position
        transform.LookAt(targetPosition);

        // Adjust the canvas rotation so it directly faces the camera
        transform.Rotate(0, 180, 0);
    }
}