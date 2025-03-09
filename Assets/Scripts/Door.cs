using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isHeld = false;
    public bool isLocked = false;
    static float soundPlayDuration = 2;
    private Coroutine PlaySound = null;
    private float timer = 0f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Lock" && !isHeld)
        {
            HingeJoint hinge = GetComponent<HingeJoint>();
            JointLimits limits = hinge.limits;
            limits.min = -1;
            hinge.limits = limits;
        }
        playSound();
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

    public void playSound()
    {
        if (PlaySound == null)
        {
            PlaySound = StartCoroutine(Sound());
        }
        else
        {
            timer = soundPlayDuration;
        }
    }

    IEnumerator Sound()
    {
        while (timer >= 0)
        {
            float velocity = gameObject.GetComponent<Rigidbody>().velocity.normalized.magnitude;
            //Sound's speed and pitch effect depends on velocity

            timer -= Time.deltaTime;
            yield return null;
        }
    }
}
