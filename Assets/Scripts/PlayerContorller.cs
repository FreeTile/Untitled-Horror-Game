using UnityEngine;

public class PlayerContorller : MonoBehaviour
{
    private Rigidbody RBody;

    [SerializeField] private Camera MCamera;
    [SerializeField] private float MovementSpeed = 5f;
    [SerializeField] private float MouseSensitivity = 100f;
    [SerializeField] private float rotationSmoothTime = 0.1f; // Smoothing factor

    // Target rotation values that accumulate mouse input
    private float targetYaw;
    private float targetPitch;

    // Velocities for SmoothDampAngle (required by the function)
    private float yawSmoothVelocity;
    private float pitchSmoothVelocity;

    void Start()
    {
        RBody = GetComponent<Rigidbody>();

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Initialize target rotations to the current rotations
        targetYaw = transform.eulerAngles.y;
        // Convert the camera's local pitch angle to a signed value (-180 to 180)
        targetPitch = MCamera.transform.localEulerAngles.x;
        if (targetPitch > 180f)
            targetPitch -= 360f;
    }

    void Update()
    {
        MoveCharacter();
        RotateCamera();
    }

    private void MoveCharacter()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        Vector3 newVelocity = new Vector3(moveDirection.x * MovementSpeed, RBody.velocity.y, moveDirection.z * MovementSpeed);
        RBody.velocity = newVelocity;
    }

    private void RotateCamera()
    {
        // Get raw mouse input (no Time.deltaTime multiplication for smoothing)
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity;

        // Update target rotations based on mouse input
        targetYaw += mouseX;
        targetPitch -= mouseY;
        targetPitch = Mathf.Clamp(targetPitch, -90f, 90f);

        // Smoothly interpolate the current yaw and pitch towards the target values
        float smoothYaw = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetYaw, ref yawSmoothVelocity, rotationSmoothTime);
        float smoothPitch = Mathf.SmoothDampAngle(GetLocalPitch(), targetPitch, ref pitchSmoothVelocity, rotationSmoothTime);

        // Apply the smoothed rotations
        transform.rotation = Quaternion.Euler(0f, smoothYaw, 0f);
        MCamera.transform.localRotation = Quaternion.Euler(smoothPitch, 0f, 0f);
    }

    // Helper method to get the camera's current pitch in a signed format (-180 to 180)
    private float GetLocalPitch()
    {
        float pitch = MCamera.transform.localEulerAngles.x;
        if (pitch > 180f)
            pitch -= 360f;
        return pitch;
    }
}
