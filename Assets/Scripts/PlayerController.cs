using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private Transform MCameraPivot;
    [SerializeField] private float MovementSpeed = 5f;
    [SerializeField] private float MouseSensitivity = 1f;

    [SerializeField] private float pushForce = 5f;  

    private float gravity = -9.8f;
    private Vector3 verticalVelocity = Vector3.zero;
    private float verticalLookRotation = 0f;

    private InputHandler input;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        input = InputHandler.Instance;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        RotateCamera();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        Vector3 moveDirection = (transform.right * input.MoveInput.x + transform.forward * input.MoveInput.y).normalized;
        float sprintMult = input.SprintInput ? 2f : 1f;

        Vector3 horizontalVelocity = moveDirection * MovementSpeed * sprintMult;

        if (controller.isGrounded)
        {
            verticalVelocity.y = -5f;
        }
        else
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }

        Vector3 velocity = horizontalVelocity + verticalVelocity;

        controller.Move(velocity * Time.deltaTime);

    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody hitRigidbody = hit.collider.attachedRigidbody;
        if (hitRigidbody != null && !hitRigidbody.isKinematic)
        {
            Vector3 contactNormal = hit.normal;
            Vector3 pushDirection = (hit.transform.position - transform.position).normalized;
            pushDirection.Normalize();
            hitRigidbody.AddForceAtPosition(pushDirection * pushForce, hit.point, ForceMode.Force);
        }
    }
    
    
    private void RotateCamera()
    {
        float mouseX = input.LookInput.x * MouseSensitivity;
        float mouseY = input.LookInput.y * MouseSensitivity;

        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        MCameraPivot.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }
}
