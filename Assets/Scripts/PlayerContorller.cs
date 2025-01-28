using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerContorller : MonoBehaviour
{
    private Rigidbody RBody;
    [SerializeField]
    private Camera MCamera;
    [SerializeField]
    private float MovementSpeed;
    [SerializeField]
    private float MouseSensitivity;
    private float verticalLookRotation = 0f;
    void Start()
    {
        RBody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        MCamera.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
        this.transform.Rotate(Vector3.up * mouseX);
    }

}
