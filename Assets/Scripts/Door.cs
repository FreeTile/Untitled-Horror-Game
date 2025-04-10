using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HingeJoint))]
public class Door : MonoBehaviour
{
    [Header("FMOD Events")]
    public EventReference doorOpenEvent;
    public EventReference doorCloseEvent;
    public EventReference doorLockedEvent;

    private bool isHeld = false;
    public bool isLocked = false;

    private Coroutine playSoundCoroutine = null;
    private HingeJoint hinge;
    private JointLimits limits;

    public float velocityThreshold = 0.1f;
    public float stopDelayDuration = 2f;
    [SerializeField]
    private float initialAngle;

    private float initialDoorAngle;
    private const float angleThreshold = 1f;

    private bool isInitialized = false;

    private void OnValidate()
    {
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void Start()
    {
        hinge = GetComponent<HingeJoint>();
        hinge.useLimits = true;
        limits.min = -1;
        limits.max = -0.1f;
        hinge.limits = limits;

        initialDoorAngle = transform.localEulerAngles.y;

        StartCoroutine(DelayedInitialization());
    }

    private IEnumerator DelayedInitialization()
    {
        yield return new WaitForSeconds(1f);
        isInitialized = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isInitialized)
            ProcessMove();
    }

    public void Open()
    {
        if (limits.min == -1)
        {
            RuntimeManager.PlayOneShot(doorOpenEvent, transform.position);
        }
        limits.min = -initialAngle;
        limits.max = -0.1f;
        hinge.limits = limits;
    }

    public void Close()
    {
        limits.min = -1;
        limits.max = -0.1f;
        hinge.limits = limits;
        RuntimeManager.PlayOneShot(doorCloseEvent, transform.position);
    }

    public void Unlock() { isLocked = false; }
    public void Lock() { isLocked = true; }

    public void grab()
    {
        if (isLocked)
        {
            RuntimeManager.PlayOneShot(doorLockedEvent, transform.position);
            return;
        }
        Debug.Log("Proceeded through lock");
        isHeld = true;
        Open();
        ProcessMove();
    }

    public void drop()
    {
        isHeld = false;
    }

    public void ProcessMove()
    {
        if (playSoundCoroutine == null)
        {
            playSoundCoroutine = StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        while (true)
        {
            if (!isInitialized)
            {
                yield return null;
                continue;
            }
            
            float velocity = GetComponent<Rigidbody>().velocity.magnitude;
            float currentAngle = transform.localEulerAngles.y;

            if (currentAngle > 359f && !isHeld && Mathf.Abs(currentAngle - initialDoorAngle) > angleThreshold)
            {
                Close();
                break;
            }
            yield return null;
        }

        playSoundCoroutine = null;
    }
}
