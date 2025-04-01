using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HingeJoint))]
public class LDoor : MonoBehaviour
{
    static int InteractLayer = 10;

    private void OnValidate()
    {
        transform.tag = "Door";
        gameObject.layer = InteractLayer;

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }
        meshCollider.convex = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.drag = 0.5f;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        Rigidbody parentRb = transform.parent.GetComponent<Rigidbody>();
        if (parentRb == null)
        {
            parentRb = transform.parent.AddComponent<Rigidbody>();
        }
        parentRb.isKinematic = true;

        HingeJoint joint = GetComponent<HingeJoint>();
        if (joint == null)
        {
            joint = gameObject.AddComponent<HingeJoint>();
        }
        joint.useLimits = true;
        joint.connectedBody = parentRb;
        JointLimits limits = joint.limits;
        limits.min = 0;
        limits.max = 135;
        joint.limits = limits;
        joint.axis = new Vector3(0, 1, 0);

    }
}
