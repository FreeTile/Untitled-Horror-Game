using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    private GameObject interactedObject = null;
    private ConfigurableJoint currentJoint = null;
    [SerializeField] private float interactDistance = 1.5f;

    [SerializeField] private Transform holdPosition;
    [SerializeField] private Camera MCamera;
    [SerializeField] private LayerMask mask;

    private InputHandler input;

    [SerializeField] private float breakDistance = 1f;
    [SerializeField] private float ThrowForce = 10f;

    private void Start()
    {
        input = InputHandler.Instance;
    }

    private void Update()
    {
        HandleInteraction();
        CheckDistance();
    }

    private void HandleInteraction()
    {
        if (input.InteractDown && interactedObject == null)
        {
                // Если объект ещё не выбран, пытаемся его обнаружить лучом
                RaycastHit hit;
                if (Physics.Raycast(MCamera.transform.position, MCamera.transform.forward, out hit, interactDistance, mask))
                {
                    switch (hit.transform.tag)
                    {
                        case "Draggable":
                                interactedObject = hit.transform.gameObject;
                                AttachJoint(interactedObject);
                            break;
                        case "Pickable":
                                PickUpItem(hit.transform.gameObject);
                            break;
                        case "Readable":
                                ReadNote(hit.transform.gameObject);
                            break;

                    }
                }

        }
        else
        {
            if (input.ThrowDown && interactedObject != null) 
            {
                ThrowObject();
            }
            if (!input.InteractHold && interactedObject != null)
            {
                BreakJoint();
            }
        }
    }

    private void CheckDistance()
    {
        if (interactedObject != null && currentJoint != null)
        {
            float distance = Vector3.Distance(interactedObject.transform.position, holdPosition.position);
            if (distance > breakDistance)
            {
                BreakJoint();
            }
        }
    }

    private void AttachJoint(GameObject obj)
    {
        Rigidbody objRb = obj.GetComponent<Rigidbody>();
        if (objRb == null) return;

        currentJoint = obj.AddComponent<ConfigurableJoint>();

        currentJoint.connectedBody = holdPosition.GetComponent<Rigidbody>();

        currentJoint.autoConfigureConnectedAnchor = false;
        currentJoint.axis = Vector3.zero;
        currentJoint.anchor = Vector3.zero;

        currentJoint.connectedAnchor = Vector3.zero;

        currentJoint.angularXMotion = ConfigurableJointMotion.Locked;
        currentJoint.angularYMotion = ConfigurableJointMotion.Locked;
        currentJoint.angularZMotion = ConfigurableJointMotion.Locked;

        currentJoint.xMotion = ConfigurableJointMotion.Free;
        currentJoint.yMotion = ConfigurableJointMotion.Free;
        currentJoint.zMotion = ConfigurableJointMotion.Free;

        SoftJointLimit linearLimit = new SoftJointLimit();
        linearLimit.limit = 0.1f;
        currentJoint.linearLimit = linearLimit;

        JointDrive drive = new JointDrive();
        drive.positionSpring = 1000f;
        drive.positionDamper = 50f;
        drive.maximumForce = 1000f;
        currentJoint.xDrive = drive;
        currentJoint.yDrive = drive;
        currentJoint.zDrive = drive;

        currentJoint.projectionMode = JointProjectionMode.PositionAndRotation;
        currentJoint.projectionDistance = 0.1f;
        currentJoint.projectionAngle = 1f;
    }

    private void BreakJoint()
    {
        if (currentJoint != null)
        {
            Destroy(currentJoint);
            currentJoint = null;
        }
        interactedObject = null;
    }

    private void ThrowObject()
    {
        if (currentJoint != null)
        {
            Destroy(currentJoint);
            currentJoint = null;
        }
        Vector3 direction = (interactedObject.transform.position - MCamera.transform.position).normalized;
        interactedObject.GetComponent<Rigidbody>().AddForce(direction * ThrowForce, ForceMode.Impulse);
        interactedObject = null;
    }

    private void PickUpItem(GameObject obj)
    {
        //code for to pick up an item
    }

    private void ReadNote(GameObject obj)
    {
        //code to read a note
    }
}
