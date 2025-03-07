using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NonConvexMeshCollider))]
public class Draggable : MonoBehaviour
{
    static int InteractLayer = 10;

    private void OnValidate()
    {
        transform.tag = "Draggable";
        gameObject.layer = InteractLayer;

        //MeshCollider meshCollider = GetComponent<MeshCollider>();
        //if (meshCollider != null)
        //{
        //    DestroyImmediate(meshCollider);
        //}

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.drag = 0.5f;

        NonConvexMeshCollider nonConvexMeshCollider = GetComponent<NonConvexMeshCollider>();
        if (nonConvexMeshCollider == null)
        {
            nonConvexMeshCollider = gameObject.AddComponent<NonConvexMeshCollider>();
        }

        nonConvexMeshCollider.createChildGameObject = true;
        nonConvexMeshCollider.boxesPerEdge = 40;
    }
}
