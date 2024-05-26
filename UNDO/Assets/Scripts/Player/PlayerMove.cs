using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    // Reference to the player's camera
    public Camera playerCamera;

    // Movement speeds
    public float walkSpeed = 6f;
    public float runSpeed = 9f;
    public float crouchSpeed = 3f;

    // Jumping parameters
    public float jumpPower = 7f;
    public float gravity = 10f;

    // Look sensitivity
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    // Height parameters
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;

    // Public variable to track inventory state
    public bool isInventoryOpen = false;

    // Private variables for movement
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private bool canMove = true;

    void Start()
    {
        // Get reference to CharacterController component
        characterController = GetComponent<CharacterController>();

        // Lock cursor and show it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    void Update()
    {
        // Ensure the cursor is always visible
        Cursor.visible = true;

        if (!isInventoryOpen)
        {
            // Get the direction of movement based on player's input
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            // Check if the player is running
            bool isRunning = Input.GetKey(KeyCode.LeftShift);

            // Calculate movement speed based on input and whether player is running or not
            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

            // Store the current vertical movement direction
            float movementDirectionY = moveDirection.y;

            // Combine the movement directions
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            // Handle jumping
            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpPower;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            // Apply gravity
            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            // Handle crouching
            if (Input.GetKey(KeyCode.LeftControl) && canMove)
            {
                characterController.height = crouchHeight;
                walkSpeed = crouchSpeed;
                runSpeed = crouchSpeed;
            }
            else
            {
                characterController.height = defaultHeight;
                walkSpeed = 6f;
                runSpeed = 9f;
            }

            // Move the player
            characterController.Move(moveDirection * Time.deltaTime);

            // Handle player rotation
            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        }
    }
}
