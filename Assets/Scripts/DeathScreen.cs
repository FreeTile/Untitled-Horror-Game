using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject restart;
    [SerializeField]
    private GameObject menu;
    [SerializeField] private TextMeshProUGUI deathScreenText;
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float eraseSpeed = 0.03f;

    public void Death()
    {
        StartCoroutine(DeathAnimation());
    }
    public IEnumerator DeathAnimation()
    {
        yield return StartCoroutine(TypeText("You are not ready to die"));
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(EraseText());
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(TypeText("Wake up"));
        yield return new WaitForSeconds(0.5f);
        restart.SetActive(true);
        menu.SetActive(true);
        yield return null;
    }

    private IEnumerator TypeText(string message)
    {
        deathScreenText.text = "";

        foreach (char letter in message)
        {
            deathScreenText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private IEnumerator EraseText()
    {
        while (deathScreenText.text.Length > 0)
        {
            deathScreenText.text = deathScreenText.text.Substring(0, deathScreenText.text.Length - 1);
            yield return new WaitForSeconds(eraseSpeed);
        }
    }

    public void Restart()
    {
        SceneTransition.SwitchToScene("Game");
    }

    public void MainMenu()
    {
        SceneTransition.SwitchToScene("Menu");
    }
}
