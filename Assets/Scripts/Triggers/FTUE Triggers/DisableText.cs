using System.Collections;
using UnityEngine;

public class DisableText : MonoBehaviour
{
    public void DisplayFor(float seconds)
    {
        gameObject.SetActive(true);
        StartCoroutine(HideAfter(seconds));
    }

    private IEnumerator HideAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}
