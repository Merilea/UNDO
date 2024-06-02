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
    public float crouchTransitionSpeed = 5f; // Speed of crouch transition

    // Public variable to track inventory state
    public bool isInventoryOpen = false;

    // Private variables for movement
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private bool canMove = true;

    private PlayerHealth playerHealth;
    private ObjectInspection objectInspection; // Reference to ObjectInspection
    private float targetHeight; // Target height for smooth crouching

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerHealth = GetComponent<PlayerHealth>();
        objectInspection = FindObjectOfType<ObjectInspection>(); // Find the ObjectInspection script

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        if (characterController == null)
        {
            Debug.LogError("CharacterController component is missing.");
        }

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth component is missing.");
        }

        if (objectInspection == null)
        {
            Debug.LogError("ObjectInspection component is missing in the scene.");
        }

        targetHeight = defaultHeight; // Initialize target height
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    void Update()
    {
        if (characterController == null || playerHealth == null || objectInspection == null)
        {
            return; // Exit update if any required component is missing
        }

        if (!objectInspection.IsInspecting() && !isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            bool isRunning = Input.GetKey(KeyCode.LeftShift);

            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

            float movementDirectionY = moveDirection.y;

            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                if (playerHealth.stamina >= 10f)
                {
                    moveDirection.y = jumpPower;
                    playerHealth.UseStamina(10f);
                }
                else
                {
                    playerHealth.TakeDamage(5f);
                }
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            if (isRunning && canMove)
            {
                if (playerHealth.stamina >= 5f)
                {
                    playerHealth.UseStamina(5f * Time.deltaTime);
                }
                else
                {
                    playerHealth.TakeDamage(5f);
                }
            }

            // Handle crouching
            if (Input.GetKey(KeyCode.LeftControl) && canMove)
            {
                targetHeight = crouchHeight;
                walkSpeed = crouchSpeed;
                runSpeed = crouchSpeed;
            }
            else
            {
                targetHeight = defaultHeight;
                walkSpeed = 6f;
                runSpeed = 9f;
            }

            // Smoothly transition the height
            characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);

            characterController.Move(moveDirection * Time.deltaTime);

            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        }
        else
        {
            // Ensure the cursor is visible and unlocked when inventory is open or inspection is happening
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
