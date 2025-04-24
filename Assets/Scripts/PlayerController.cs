using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private Transform MCameraPivot;
    [SerializeField] private float MovementSpeed = 5f;
    [SerializeField] public float MouseSensitivity = 1f;

    [SerializeField] private float pushForce = 5f;  

    private float gravity = -9.8f;
    private Vector3 verticalVelocity = Vector3.zero;
    private float verticalLookRotation = 0f;

    private GameInputHandler input;

    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float crouchDuration = 0.2f;
    [SerializeField] private float crouchCameraOffset = -0.5f;
    [SerializeField] private float CrouchSpeedMulti = 0.5f;

    private Tweener heightTween;
    private Tweener cameraTween;
    private float initialCameraY;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        input = GameInputHandler.Instance;
        initialCameraY = MCameraPivot.localPosition.y;
        MouseSensitivity = PlayerPrefs.GetFloat("Sensitivity");
    }

    private void Update()
    {
        RotateCamera();
        if (input.CrouchDown)
        {
            ChangeState();
        }
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        Vector3 moveDirection = (transform.right * input.MoveInput.x + transform.forward * input.MoveInput.y).normalized;
        float sprintMult = input.SprintInput ? 2f : 1f;

        float Speed = MovementSpeed;
        if (input.isCrouching)
        {
            Speed = MovementSpeed * CrouchSpeedMulti;
        }
        else
        {
            Speed = MovementSpeed * sprintMult;
        }

        Vector3 horizontalVelocity = moveDirection * Speed;

        if (controller.isGrounded)
        {
            verticalVelocity.y = gravity;
        }
        else
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }

        Vector3 velocity = horizontalVelocity + verticalVelocity;

        controller.Move(velocity * Time.deltaTime);

    }

    void ChangeState()
    {
        RaycastHit hit;
        if (Physics.Raycast(MCameraPivot.transform.position, Vector3.up, standHeight - crouchHeight)) { return; }
        input.isCrouching = !input.isCrouching;
        float targetHeight = input.isCrouching ? crouchHeight : standHeight;
        float targetCameraY = input.isCrouching ? initialCameraY + crouchCameraOffset : initialCameraY;

        heightTween?.Kill();
        cameraTween?.Kill();

        heightTween = DOTween.To(
            () => controller.height,
            x => controller.height = x,
            targetHeight,
            crouchDuration
        );

        cameraTween = MCameraPivot.DOLocalMoveY(targetCameraY, crouchDuration);
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
        //Debug.Log("Delta: " + input.LookInput.magnitude * MouseSensitivity);
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        MCameraPivot.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }
}
