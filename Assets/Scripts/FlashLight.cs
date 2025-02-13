using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    private GameInputHandler input;
    [SerializeField] private GameObject FLObject, FLModel;
    [SerializeField] private float autoAimMinDistance = 1f;
    [SerializeField] private float followDelay = 0.2f;
    [SerializeField] private Camera MCamera;
    [SerializeField] private LayerMask aimMask;

    [SerializeField] private Light lightSource;
    [SerializeField] private float MinAngle, MaxAngle;
    [SerializeField] private float maxIntensity = 6f;
    [SerializeField] private float minIntensity = 2f;
    private static float Charge = 120f;
    private Animator anim;
    private bool isOn = false;
    
    void Start()
    {
        input = GameInputHandler.Instance;
        float t = (lightSource.innerSpotAngle - MinAngle) / (MaxAngle - MinAngle);
        lightSource.intensity = Mathf.Lerp(maxIntensity, minIntensity, t);
        anim = FLObject.GetComponent<Animator>();
    }

    void Update()
    {
        TurnOnOffLight();
        ConfigureRadius();
    }

    private void FixedUpdate()
    {
        DecreaseCharge();
    }

    private void LateUpdate()
    {
        if (isOn)
        {
            AutoAim();
        }

    }
    private void TurnOnOffLight()
    {
        if (input.FlashlightDown)
        {
            if (!isOn && Charge > 0f)
            {
                lightSource.enabled = true;
                anim.SetBool("IsOn", true);
                isOn = true;
            }
            else
            {             
                lightSource.enabled = false;
                anim.SetBool("IsOn", false);
                isOn = false;
            }
        }
    }

    private void AutoAim()
    {
        Quaternion targetRotation;
        RaycastHit hit;
        if (Physics.Raycast(MCamera.transform.position, MCamera.transform.forward, out hit, 20f, aimMask))
        {
            if (hit.distance > autoAimMinDistance)
            {
                Vector3 direction = hit.point - FLModel.transform.position;
                targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            }
            else
            {
                targetRotation = MCamera.transform.rotation;
            }
        }
        else
        {
            targetRotation = MCamera.transform.rotation;
        }
        targetRotation = targetRotation * Quaternion.Euler(0, 180, 0);
        FLModel.transform.rotation = Quaternion.Lerp(FLModel.transform.rotation, targetRotation, Time.deltaTime / followDelay);
    }

    private void ConfigureRadius()
    {
        float changeValue = input.WheelInput;
        if (changeValue != 0)
        {
            lightSource.innerSpotAngle = Mathf.Clamp(lightSource.innerSpotAngle + changeValue / 120f, MinAngle, MaxAngle);
            lightSource.spotAngle = Mathf.Clamp(lightSource.spotAngle + changeValue / 120f, MinAngle + 25, MaxAngle + 25f);
            float t = (lightSource.innerSpotAngle - MinAngle) / (MaxAngle - MinAngle);
            lightSource.intensity = Mathf.Lerp(maxIntensity, minIntensity, t);
        }
    }

    private void DecreaseCharge()
    {
        if (Charge > 0)
        {
            Charge -= Time.deltaTime;
        }
        else if (isOn)
        {
            lightSource.enabled = false;
            anim.SetBool("IsOn", false);
            isOn = false;
        }
    }

    public static void IncreaseCharge(float value)
    {
        Charge += value;
    }
}
