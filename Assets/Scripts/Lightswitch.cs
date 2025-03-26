using UnityEngine;

public class Lightswitch : MonoBehaviour
{
    // The light you want to control. Assign this in the Inspector.
    [SerializeField] private Light controlledLight;

    // True when the player is within the switch's trigger collider.
    private bool playerInRange = false;

    // Called when another collider enters this trigger collider.
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player.
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    // Called when another collider exits this trigger collider.
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    // Update is called once per frame.
    private void Update()
    {
        // Check if the player is in range and has pressed the interact button.
        if (playerInRange && GameInputHandler.Instance.InteractDown)
        {
            ToggleLight();
        }
    }

    // Toggles the light on/off.
    private void ToggleLight()
    {
        if (controlledLight != null)
        {
            controlledLight.enabled = !controlledLight.enabled;
        }
        else
        {
            Debug.LogWarning("Controlled Light is not assigned on " + gameObject.name);
        }
    }
}
