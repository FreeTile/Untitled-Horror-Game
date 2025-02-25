using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    private GameObject interactedObject = null;
    private ConfigurableJoint current—Joint = null;
    private SpringJoint currentDoorSpringJoint = null;
    private Vector3 initialHoldPos;
    [SerializeField] private float interactDistance = 1.5f;

    [SerializeField] private Transform holdPosition;
    GameObject dragPointGameobject;
    private int leftDoor = 0;
    [SerializeField] private Camera MCamera;
    [SerializeField] private LayerMask mask;

    private GameInputHandler input;

    [SerializeField] private float breakDistance = 1f;
    [SerializeField] private float ThrowForce = 10f;

    private void Start()
    {
        input = GameInputHandler.Instance;
        initialHoldPos = holdPosition.transform.localPosition;
    }

    private void Update()
    {
        HandleInteraction();
        CheckDistance();
    }

    private void HandleInteraction()
    {
        if (input.InteractDown && interactedObject == null) //LMB
        {
            RaycastHit hit; //Casting a ray to check if there is an object in front of the camera
            if (Physics.Raycast(MCamera.transform.position, MCamera.transform.forward, out hit, interactDistance, mask))
            {
                switch (hit.transform.tag)
                {
                    case "Draggable": //All draggable objects
                        interactedObject = hit.transform.gameObject;
                        AttachJoint(interactedObject);
                        break;
                    case "Door": //Doors + drawers
                        interactedObject = hit.transform.gameObject;
                        AttachDoorJoint(interactedObject, hit.point);
                        break;
                    case "Pickable": //Batteries + pills
                        PickUpItem(hit.transform.gameObject);
                        break;
                    case "Readable": //Notes
                        ReadNote(hit.transform.gameObject);
                        break;
                }
            }
        }
        else if (interactedObject != null) //throw an object
        {
            if (input.ThrowDown)
            {
                ThrowObject();
            }
            else if (!input.InteractHold)
            {
                if (current—Joint != null)
                    BreakJoint();
                if (currentDoorSpringJoint != null)
                    BreakDoorJoint();
            }
        }
    }

    //If the distance between the holding position and the object is too large, we break up the connection -> throws the object
    private void CheckDistance() 
    {
        if (interactedObject != null && current—Joint != null)
        {
            float distance = Vector3.Distance(interactedObject.transform.position, holdPosition.position);
            if (distance > breakDistance)
            {
                BreakJoint();
            }
        }
    }

    //Attaching and tuning a configurable joint for draggable objects (Check configurable joint as a component in Unity for more info)
    private void AttachJoint(GameObject obj)
    {
        Rigidbody objRb = obj.GetComponent<Rigidbody>();
        if (objRb == null) return;

        current—Joint = obj.AddComponent<ConfigurableJoint>();

        current—Joint.connectedBody = holdPosition.GetComponent<Rigidbody>();

        current—Joint.autoConfigureConnectedAnchor = false;
        current—Joint.axis = Vector3.zero;
        current—Joint.anchor = Vector3.zero;

        current—Joint.connectedAnchor = Vector3.zero;

        current—Joint.angularXMotion = ConfigurableJointMotion.Locked;
        current—Joint.angularYMotion = ConfigurableJointMotion.Locked;
        current—Joint.angularZMotion = ConfigurableJointMotion.Locked;

        current—Joint.xMotion = ConfigurableJointMotion.Free;
        current—Joint.yMotion = ConfigurableJointMotion.Free;
        current—Joint.zMotion = ConfigurableJointMotion.Free;

        SoftJointLimit linearLimit = new SoftJointLimit();
        linearLimit.limit = 0.1f;
        current—Joint.linearLimit = linearLimit;

        JointDrive drive = new JointDrive();
        drive.positionSpring = 1000f;
        drive.positionDamper = 50f;
        drive.maximumForce = 1000f;
        current—Joint.xDrive = drive;
        current—Joint.yDrive = drive;
        current—Joint.zDrive = drive;

        current—Joint.projectionMode = JointProjectionMode.PositionAndRotation;
        current—Joint.projectionDistance = 0.1f;
        current—Joint.projectionAngle = 1f;
    }

    //Deleting joint from the last held object
    private void BreakJoint()
    {
        if (current—Joint != null)
        {
            Destroy(current—Joint);
            current—Joint = null;
        }
        interactedObject = null;
    }

    //Attaching and tuning a spring joint for door like objects (Check spring joint as a component in Unity for more info)
    //For doors we use a hinge joint in additional to a spring joint
    //For drowers we use a configurable joint in additional to a spring joint
    private void AttachDoorJoint(GameObject door, Vector3 hitPoint)
    {
        Rigidbody doorRb = door.GetComponent<Rigidbody>();
        if (doorRb == null) return;

        holdPosition.transform.position = hitPoint;

        currentDoorSpringJoint = door.AddComponent<SpringJoint>();
        currentDoorSpringJoint.autoConfigureConnectedAnchor = false;
        Rigidbody holdRb = holdPosition.GetComponent<Rigidbody>();
        if (holdRb == null)
        {
            holdRb = holdPosition.gameObject.AddComponent<Rigidbody>();
            holdRb.isKinematic = true;
        }
        currentDoorSpringJoint.connectedBody = holdRb;

        Vector3 localHitPoint = door.transform.InverseTransformPoint(hitPoint);
        currentDoorSpringJoint.anchor = localHitPoint;
        currentDoorSpringJoint.connectedAnchor = Vector3.zero;

        currentDoorSpringJoint.spring = 50f;
        currentDoorSpringJoint.damper = 25f;
        currentDoorSpringJoint.minDistance = 0f;
        currentDoorSpringJoint.maxDistance = 0f;
    }

    //Breaking spring joint from the las held doorlike object
    private void BreakDoorJoint()
    {
        if (currentDoorSpringJoint != null)
        {
            Destroy(currentDoorSpringJoint);
            currentDoorSpringJoint = null;
        }
        interactedObject = null;
        holdPosition.transform.localPosition = initialHoldPos;
    }

    //Throwing an object with force from the player
    private void ThrowObject()
    {
        if (current—Joint != null)
        {
            Destroy(current—Joint);
            current—Joint = null;
        }
        Vector3 direction = (interactedObject.transform.position - MCamera.transform.position).normalized;
        interactedObject.GetComponent<Rigidbody>().AddForce(direction * ThrowForce, ForceMode.Impulse);
        interactedObject = null;
    }

    private void PickUpItem(GameObject obj)
    {
        //code to pick up an item
    }

    private void ReadNote(GameObject obj)
    {
        //code to read a note
    }
}
