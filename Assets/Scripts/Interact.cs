using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.Shapes;

public class Interact : MonoBehaviour
{
    private ItemSO item;
    private PlayerInventory inventory;
    private GameObject interactedObject = null;
    private ConfigurableJoint currentÑJoint = null;
    private SpringJoint currentDoorSpringJoint = null;
    private Vector3 initialHoldPos;
    [SerializeField] private float interactDistance = 1.5f;

    [SerializeField] private Transform holdPosition;
    [SerializeField] private float positionSpring = 100f;
    [SerializeField] private float positionDamper = 50f;
    [SerializeField] private float angularSpring = 100f;
    [SerializeField] private float angularDamper = 20f;
    [SerializeField] private Camera MCamera;
    [SerializeField] private LayerMask mask;

    private GameInputHandler input;

    [SerializeField] private float breakDistance = 1f;
    [SerializeField] private float ThrowForce = 10f;
    [SerializeField] private Animator animator;

    private Door door = null;

    private void Start()
    {
        input = GameInputHandler.Instance;
        initialHoldPos = holdPosition.transform.localPosition;
        inventory = GetComponent<PlayerInventory>();
        initialHoldPos = holdPosition.transform.localPosition;
    }

       private void Update()
    {
        HandleInteraction();
        CheckDistance();
    }

    private void HandleInteraction()
    {
        if (interactedObject == null) //LMB
        {
            RaycastHit hit; //Casting a ray to check if there is an object in front of the camera
            if (Physics.Raycast(MCamera.transform.position, MCamera.transform.forward, out hit, interactDistance, mask))
            {
                animator.SetBool("Holdable", true);
                switch (hit.transform.tag)
                {
                    case "Draggable": //All draggable objects
                        if (input.InteractDown)
                        {
                            animator.SetBool("Holded", true);
                            interactedObject = hit.transform.gameObject;
                            AttachJoint(interactedObject, hit.point);
                        }
                        break;
                    case "Door": //Doors + drawers
                        if (input.InteractDown)
                        {
                            animator.SetBool("Holded", true);
                            interactedObject = hit.transform.gameObject;
                            door = interactedObject.GetComponent<Door>();
                            if(door != null) door.grab();
                            AttachDoorJoint(interactedObject, hit.point);
                        }
                        break;
                    case "Pickable": //Batteries + pills
                        if (input.InteractDown)
                        {
                            animator.SetBool("Holded", true);
                            PickUpItem(hit.transform.gameObject);
                        }
                        break;
                    case "Readable": //Notes
                        if (input.InteractDown)
                        {
                            animator.SetBool("Holded", true);
                            ReadNote(hit.transform.gameObject);
                        }
                        break;
                }
            }
            else
            {
                animator.SetBool("Holdable", false);
            }
        }
        else if (interactedObject != null) //throw an object
        {

            if (input.ThrowDown)
            {
                ThrowObject();
                animator.SetTrigger("Throw");
                animator.SetBool("Holded", false);
            }
            else if (!input.InteractHold)
            {
                if (currentÑJoint != null)
                    BreakJoint();
                if (currentDoorSpringJoint != null)
                    BreakDoorJoint();
                animator.SetBool("Holded", false);
            }
        }
    }

    //If the distance between the holding position and the object is too large, we break up the connection -> throws the object
    private void CheckDistance()
    {
        if (interactedObject != null)
        {
            if (currentDoorSpringJoint != null)
            {
                Vector3 doorAnchor= interactedObject.transform.TransformPoint(currentDoorSpringJoint.anchor);
                Vector3 holdAnchor = holdPosition.transform.TransformPoint(currentDoorSpringJoint.connectedAnchor);
                float distance = Vector3.Distance(doorAnchor, holdAnchor);
                if (distance > breakDistance)
                {
                    animator.SetBool("Holded", false);
                    BreakDoorJoint();
                }
            }
            else if (currentÑJoint != null)
            {
                float distance = Vector3.Distance(interactedObject.transform.position, holdPosition.position);
                if (distance > breakDistance)
                {
                    animator.SetBool("Holded", false);
                    BreakJoint();
                }
            }
        }
    }

    //Attaching and tuning a configurable joint for draggable objects (Check configurable joint as a component in Unity for more info)
    private void AttachJoint(GameObject obj, Vector3 hitPoint)
    {
        Rigidbody objRb = obj.GetComponent<Rigidbody>();
        holdPosition.position = hitPoint;
        if (objRb == null) return;

        currentÑJoint = obj.AddComponent<ConfigurableJoint>();

        currentÑJoint.connectedBody = holdPosition.GetComponent<Rigidbody>();

        currentÑJoint.autoConfigureConnectedAnchor = false;
        currentÑJoint.axis = Vector3.zero;
        Vector3 localHitPoint = obj.transform.InverseTransformPoint(hitPoint);
        currentÑJoint.anchor = localHitPoint;
        currentÑJoint.connectedAnchor = Vector3.zero;

        currentÑJoint.angularXMotion = ConfigurableJointMotion.Limited;
        currentÑJoint.angularYMotion = ConfigurableJointMotion.Limited;
        currentÑJoint.angularZMotion = ConfigurableJointMotion.Limited;

        currentÑJoint.xMotion = ConfigurableJointMotion.Limited;
        currentÑJoint.yMotion = ConfigurableJointMotion.Limited;
        currentÑJoint.zMotion = ConfigurableJointMotion.Limited;

        SoftJointLimit linearLimit = new SoftJointLimit();
        linearLimit.limit = 0.1f;
        currentÑJoint.linearLimit = linearLimit;

        JointDrive drive = new JointDrive();
        drive.positionSpring = positionSpring;
        drive.positionDamper = positionDamper;
        drive.maximumForce = 1000f;
        currentÑJoint.xDrive = drive;
        currentÑJoint.yDrive = drive;
        currentÑJoint.zDrive = drive;

        SoftJointLimit angularLimit = new SoftJointLimit();
        angularLimit.limit = 180f;

        currentÑJoint.lowAngularXLimit = angularLimit;
        currentÑJoint.highAngularXLimit = angularLimit;
        currentÑJoint.angularYLimit = angularLimit;
        currentÑJoint.angularZLimit = angularLimit;

        JointDrive angularDrive = new JointDrive();
        angularDrive.positionSpring = angularSpring;
        angularDrive.positionDamper = angularDamper;
        angularDrive.maximumForce = 1000f;

        currentÑJoint.angularXDrive = angularDrive;
        currentÑJoint.angularYZDrive = angularDrive;

        currentÑJoint.projectionMode = JointProjectionMode.PositionAndRotation;
        currentÑJoint.projectionDistance = 0.1f;
        currentÑJoint.projectionAngle = 1f;
    }

    //Deleting joint from the last held object
    private void BreakJoint()
    {
        if (currentÑJoint != null)
        {
            Destroy(currentÑJoint);
            currentÑJoint = null;
        }
        interactedObject = null;
        holdPosition.transform.localPosition = initialHoldPos;
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

        currentDoorSpringJoint.spring = 100f;
        currentDoorSpringJoint.damper = 50f;
        currentDoorSpringJoint.minDistance = 0f;
        currentDoorSpringJoint.maxDistance = 0f;
    }

    //Breaking spring joint from the las held doorlike object
    private void BreakDoorJoint()
    {
        door = interactedObject.GetComponent<Door>();
        if (door != null) door.drop();
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
        if (currentÑJoint != null)
        {
            Destroy(currentÑJoint);
            currentÑJoint = null;
        }
        Vector3 direction = MCamera.transform.forward;
        interactedObject.GetComponent<Rigidbody>().AddForce(direction * ThrowForce, ForceMode.Impulse);
        interactedObject = null;
        holdPosition.transform.localPosition = initialHoldPos;
    }

    private void PickUpItem(GameObject obj)
    {
        item = obj.GetComponent<ItemType>().ItemInfo;
        inventory.AddToInventory(item);
        Destroy(obj);
    }

    private void ReadNote(GameObject obj)
    {
        //code to read a note
    }
}
