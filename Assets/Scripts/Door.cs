using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isHeld = false;
    public bool isLocked = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Lock" && !isHeld)
        {
            HingeJoint hinge = GetComponent<HingeJoint>();
            JointLimits limits = hinge.limits;
            limits.min = -1;
            hinge.limits = limits;
        }
    }
    
    public void Open()
    {
        HingeJoint hinge = GetComponent<HingeJoint>();
        JointLimits limits = hinge.limits;
        limits.min = -135;
        hinge.limits = limits;
    }

    public void Unlock()
    {

    }

    public void Lock()
    {

    }
}
