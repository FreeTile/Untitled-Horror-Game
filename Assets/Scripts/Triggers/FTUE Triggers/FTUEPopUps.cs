using UnityEngine;
using TMPro;

public class FTUEPopUps : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private string message = "Text here";
    [SerializeField] private float displayDuration = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.text = message;

            DisableText textDisplay = text.GetComponent<DisableText>();
            if (textDisplay != null)
            {
                textDisplay.DisplayFor(displayDuration);
            }
            else
            {
                Debug.LogWarning("Not Found: TextDisplay.");
            }

            Destroy(gameObject);
        }
    }

    public void DisplayTextManually(TMP_Text text, string message)
    {
        text.text = message;
        DisableText textDisplay = text.GetComponent<DisableText>();
        if (textDisplay != null)
        {
            textDisplay.DisplayFor(displayDuration);
        }
        else
        {
            Debug.LogWarning("Not Found: TextDisplay.");
        }
    }
}
