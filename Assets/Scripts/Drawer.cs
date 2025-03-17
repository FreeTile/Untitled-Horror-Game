using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NonConvexMeshCollider))]
[RequireComponent(typeof(ConfigurableJoint))]
public class Drawer : MonoBehaviour
{
    static int InteractLayer = 10;

    private void OnValidate()
    {
        transform.tag = "Door";
        gameObject.layer = InteractLayer;

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            //DestroyImmediate(meshCollider);
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.drag = 0.5f;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        Rigidbody parentRb = transform.parent.GetComponent<Rigidbody>();
        if (parentRb == null)
        {
            parentRb = transform.parent.AddComponent<Rigidbody>();
        }
        parentRb.isKinematic = true;
        parentRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        ConfigurableJoint joint = GetComponent<ConfigurableJoint>();
        if (joint == null)
        {
            joint = gameObject.AddComponent<ConfigurableJoint>();
        }
        joint.connectedBody = parentRb;
        Vector3 anchor = joint.connectedAnchor;
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = anchor;
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;
        joint.enableCollision = true;
        joint.axis = new Vector3(0, 0, 1);

        NonConvexMeshCollider nonConvexMeshCollider = GetComponent<NonConvexMeshCollider>();
        if (nonConvexMeshCollider == null)
        {
            nonConvexMeshCollider = gameObject.AddComponent<NonConvexMeshCollider>();
        }

        nonConvexMeshCollider.createChildGameObject = true;
        nonConvexMeshCollider.boxesPerEdge = 40;
    }
}
