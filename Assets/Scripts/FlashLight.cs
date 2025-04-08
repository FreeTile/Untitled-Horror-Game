using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] private Image batteryFillImage;
    [SerializeField] private TMP_Text flashlightDeadPopupText;
    private static float Charge = 30f; //120f was
    private Animator anim;
    private bool isOn = false;
    private bool popupShown = false;

    public bool isPickedUp = false;

    [Header("(FMOD) path Settings")]
    public FMODUnity.EventReference m_EventPath;

    void Start()
    {
        input = GameInputHandler.Instance;
        float t = (lightSource.innerSpotAngle - MinAngle) / (MaxAngle - MinAngle);
        lightSource.intensity = Mathf.Lerp(maxIntensity, minIntensity, t);
        anim = FLObject.GetComponent<Animator>();
        if (flashlightDeadPopupText != null)
        {
            flashlightDeadPopupText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        TurnOnOffLight();
        ConfigureRadius();
    }

    //Decreasing the charge in the fixedupdate function
    //so redusing speed is equal no matter how many frames per second the player has.
    private void FixedUpdate()
    {
        DecreaseCharge();
        if (batteryFillImage != null) batteryFillImage.fillAmount = Charge / 30f; //Flashlight max 
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
        if (isPickedUp)
        {
            if (input.FlashlightDown)
            {
                if (Charge > 0f)
                {
                    if (!isOn)
                    {
                        lightSource.enabled = true;
                        anim.SetBool("IsOn", true);
                        isOn = true;
                        PlaySound();
                    }
                    else
                    {
                        lightSource.enabled = false;
                        anim.SetBool("IsOn", false);
                        isOn = false;
                        PlaySound();
                    }
                }
                else
                {
                    lightSource.enabled = false;
                    anim.SetBool("IsOn", false);
                    isOn = false;
                }
            }
        }
    }

    //Aiming to the center of screen
    private void AutoAim()
    {
        Quaternion targetRotation;
        RaycastHit hit; //Check if there is an object in a range in front of the player (default 1-20 meters)
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

    //Setting up angles when player uses mouse wheel
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

    //Calls every fixedUpdate, reduces flashlight charge (only when the flashlight is on),
    //can be restored upon calling IncreaseCharge function
    private void DecreaseCharge()
    {
        if (isOn && Charge > 0)
        {
            Charge -= Time.deltaTime;
            if (Charge <= 0)
            {
                Charge = 0;
                lightSource.enabled = false;
                anim.SetBool("IsOn", false);
                isOn = false;
                if (!popupShown && flashlightDeadPopupText != null)
                {
                    popupShown = true;
                    StartCoroutine(ShowFlashlightDeadPopup());
                }
            }
        }
    }

    public static void IncreaseCharge(float value)
    {
        Charge += value;
    }

    //NEW: Plays the sound 
    void PlaySound()
    {
        if (Charge <= 0) return;
        FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance(m_EventPath);
        instance.start();
        instance.release();
    }

    // Coroutine to show the popup text for a few seconds
    private IEnumerator ShowFlashlightDeadPopup()
    {
        flashlightDeadPopupText.text = "Flashlight is dead. Gotta find some batteries.";
        flashlightDeadPopupText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        flashlightDeadPopupText.gameObject.SetActive(false);
    }
}
