using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.N3DS;

public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of character movement
    public float jumpForce = 300f; // Force applied for jumping
    public Transform playerModel; // The player model's transform
    public Transform cameraTransform; // The camera's transform
    public Animator animator; // Reference to the Animator component

    private Rigidbody rb;
    private bool isGrounded; // Track if the player is on the ground

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        if (rb != null)
        {
            // Freeze all rotations
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        
    }

    void Update()
    {
        // Handle animations in Update since it's independent of physics
        Vector2 move = UnityEngine.N3DS.GamePad.CirclePad;
        // Handle jumping
        if ((UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.A) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            Jump();
        }
        if (move.magnitude < 0.1f)
        {
            animator.SetBool("IsWalk", false);
            return;
        }
        animator.SetBool("IsWalk", true);

        bool isBoosting = UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.B);
        animator.SetBool("IsRun", isBoosting);

        
    }

    void FixedUpdate()
    {
        Vector2 move = UnityEngine.N3DS.GamePad.CirclePad;

        if (move.magnitude < 0.1f) return;

        bool isBoosting = UnityEngine.N3DS.GamePad.GetButtonHold(N3dsButton.B);
        float currentSpeed = isBoosting ? moveSpeed * 2f : moveSpeed;

        // Calculate move direction relative to the camera
        Vector3 moveDirection = new Vector3(move.x, 0f, move.y);
        moveDirection = cameraTransform.TransformDirection(moveDirection); // Convert to world space
        moveDirection.y = 0f; // Flatten movement to avoid affecting vertical axis
        moveDirection.Normalize();

        // Smoothly rotate the player model toward the move direction
        if (moveDirection != Vector3.zero)
        {
            float targetYRotation = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg; // Calculate Y-axis rotation
            Quaternion targetRotation = Quaternion.Euler(0f, targetYRotation, 0f); // Restrict rotation to Y-axis
            playerModel.rotation = Quaternion.Slerp(playerModel.rotation, targetRotation, Time.deltaTime * 10f); // Adjust 10f for desired rotation speed
        }

        // Move the player
        Vector3 movement = moveDirection * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        animator.SetTrigger("Jump"); // Play jump animation if available
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        // Check if the player is grounded by colliding with the ground layer
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
