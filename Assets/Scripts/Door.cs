using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class Door : MonoBehaviour
{
    [Header("FMOD Events")]
    public EventReference doorOpenEvent;
    public EventReference doorCloseEvent;
    public EventReference doorCreakLoopEvent;

    private bool isHeld = false;
    public bool isLocked = false;

    private Coroutine playSoundCoroutine = null;
    private EventInstance doorCreakInstance;

    private HingeJoint hinge;
    private JointLimits limits;

    public float velocityThreshold = 0.1f;
    public float stopDelayDuration = 2f;

    [SerializeField]
    private float initialAngle;

    private void Start()
    {
        hinge = GetComponent<HingeJoint>();
        hinge.useLimits = true;
        limits = hinge.limits;
    }

    void Update()
    {
        if (playSoundCoroutine != null && doorCreakInstance.isValid())
        {
            doorCreakInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ProcessMove();
    }

    public void Open()
    {
        if (isLocked) return;
        if (limits.max == 1)
        {
            RuntimeManager.PlayOneShot(doorOpenEvent, transform.position);
        }
        limits.min = 0;
        limits.max = initialAngle;
        hinge.limits = limits;
    }

    public void Close()
    {
        limits.min = 0;
        limits.max = 1;
        hinge.limits = limits;
        RuntimeManager.PlayOneShot(doorCloseEvent, transform.position);
    }

    public void Unlock() { isLocked = false; }
    public void Lock() { isLocked = true; }

    public void grab()
    {
        if (isLocked) return;
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
        Debug.Log("Coroutine Sound() Started.");
        doorCreakInstance = RuntimeManager.CreateInstance(doorCreakLoopEvent);
        RuntimeManager.AttachInstanceToGameObject(doorCreakInstance, transform, GetComponent<Rigidbody>());
        doorCreakInstance.start();

        float stopDelay = stopDelayDuration;
        while (true)
        {
            float velocity = GetComponent<Rigidbody>().velocity.magnitude;

            float angle = transform.localEulerAngles.y;

            if (angle < 1f && !isHeld)
            {
                Close();
                break;
            }

            if (velocity >= velocityThreshold)
            {
                doorCreakInstance.setParameterByName("Speed", velocity);
                stopDelay = stopDelayDuration;
            }
            else
            {
                doorCreakInstance.setParameterByName("Speed", 0f);
                stopDelay -= Time.deltaTime;
                if (stopDelay <= 0f)
                {
                    break;
                }
            }
            hinge.limits = limits;
            yield return null;
        }

        Debug.Log("Stopping Sound");
        doorCreakInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        doorCreakInstance.release();
        playSoundCoroutine = null;
    }
}
