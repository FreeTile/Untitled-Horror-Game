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

    public bool isHeld = false;
    public bool isLocked = false;

    private Coroutine playSoundCoroutine = null;
    private EventInstance doorCreakInstance;

    public float velocityThreshold = 0.1f;
    public float stopDelayDuration = 0.5f;

    void Update()
    {
        if (playSoundCoroutine != null && doorCreakInstance.isValid())
        {
            doorCreakInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Lock") && !isHeld)
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
        // RuntimeManager.PlayOneShot(doorOpenEvent, transform.position);
    }

    public void Unlock() { }
    public void Lock() { }

    public void playSound()
    {
        if (playSoundCoroutine == null)
        {
            playSoundCoroutine = StartCoroutine(Sound());
        }
    }

    IEnumerator Sound()
    {
        Debug.Log("Coroutine Sound() Started.");
        doorCreakInstance = RuntimeManager.CreateInstance(doorCreakLoopEvent);
        RuntimeManager.AttachInstanceToGameObject(doorCreakInstance, transform, GetComponent<Rigidbody>());
        doorCreakInstance.start();

        float stopDelay = stopDelayDuration;

        while (true)
        {
            float velocity = GetComponent<Rigidbody>().velocity.magnitude;
            // Обновляем параметр "Speed", который может влиять, например, на pitch
            doorCreakInstance.setParameterByName("Speed", velocity);

            // Если скорость ниже порога, начинаем отсчет времени до остановки
            if (velocity < velocityThreshold)
            {
                stopDelay -= Time.deltaTime;
                if (stopDelay <= 0f)
                {
                    break;
                }
            }
            else
            {

                stopDelay = stopDelayDuration;
            }

            yield return null;
        }

        Debug.Log("Stopping Sound");
        doorCreakInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        doorCreakInstance.release();
        playSoundCoroutine = null;
    }
}