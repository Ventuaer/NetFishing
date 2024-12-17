using UnityEngine;

public class DynamicTail : MonoBehaviour
{
    [Header("Tail Settings")]
    public Transform[] tailBones; // Assign TailBase, TailMiddle, TailEnd here
    public float stiffness = 5f;
    public float damping = 2f;
    public float gravity = 0.5f;
    public float maxAngle = 45f;

    private Vector3[] previousPositions;
    private Vector3[] velocities;

    void Start()
    {
        if (tailBones == null || tailBones.Length == 0)
        {
            Debug.LogError("Tail bones not assigned! Please assign them in the Inspector.");
            enabled = false; // Disable the script to avoid further errors
            return;
        }

        previousPositions = new Vector3[tailBones.Length];
        velocities = new Vector3[tailBones.Length];
        for (int i = 0; i < tailBones.Length; i++)
        {
            previousPositions[i] = tailBones[i].position;
        }
    }

    void LateUpdate()
    {
        if (tailBones.Length < 2) return; // We need at least two bones to apply movement

        Vector3 characterMovement = transform.position - previousPositions[0];
        previousPositions[0] = transform.position;

        for (int i = 1; i < tailBones.Length; i++)
        {
            Vector3 targetPosition = tailBones[i - 1].position + characterMovement;

            // Apply physics-based forces
            Vector3 force = (targetPosition - tailBones[i].position) * stiffness;
            force -= velocities[i] * damping;
            force += Vector3.down * gravity;

            // Update velocity and position
            velocities[i] += force * Time.deltaTime;
            tailBones[i].position += velocities[i] * Time.deltaTime;

            // Limit the angle between bones
            Vector3 direction = tailBones[i].position - tailBones[i - 1].position;
            direction = Vector3.ClampMagnitude(direction, Vector3.Distance(tailBones[i].position, tailBones[i - 1].position));
            tailBones[i].position = tailBones[i - 1].position + direction;

            // Optional: Smooth rotation to face the next bone
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            tailBones[i - 1].rotation = Quaternion.Slerp(tailBones[i - 1].rotation, targetRotation, Time.deltaTime * stiffness);
        }
    }
}
