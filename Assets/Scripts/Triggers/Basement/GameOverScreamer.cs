using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using FMODUnity;
public class GameOverScreamer : MonoBehaviour
{
    [Header("Monster settings")]
    public GameObject monster;
    public GameObject player;
    public string attackAnimation = "Attack";

    [Header("UI Settings")]
    public Image fadeImage;
    public TMP_Text gameOverText;

    private Animator monsterAnimator;

    public EventReference typeSound;


    void Start()
    {
        if (monster != null)
        {
            monsterAnimator = monster.GetComponent<Animator>();
            monster.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Monster hasn't assign in inspector");
        }

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
        else
        {
            Debug.LogWarning("Fade Image hasn't assign in inspector");
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Game Over Text hasn't assign in inspector");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            GameManager.Instance.state = GameManager.State.GameOver;
            GameManager.Instance.TriggerJumpScare();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            StartCoroutine(PlayAttackAndGameOver());
            
        }
    }

    IEnumerator PlayAttackAndGameOver()
    {
        monster.SetActive(true);
        Vector3 direction = player.transform.position - monster.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        monster.transform.rotation = lookRotation * Quaternion.Euler(0, 90f, 0);
        monsterAnimator.Play(attackAnimation);

        yield return new WaitForSeconds(0.2f);


        Color initialColor = fadeImage.color;
        initialColor.a = 1f;
        fadeImage.color = initialColor;

        yield return new WaitForSeconds(2f);

        gameOverText.gameObject.SetActive(true);
        yield return StartCoroutine(TypeText("Demo is Over\r\nThanks for playing!"));

        yield return new WaitForSeconds(5f);

        SceneTransition.SwitchToScene("Menu");
    }

    private IEnumerator TypeText(string message)
    {
        gameOverText.text = "";

        foreach (char letter in message)
        {
            RuntimeManager.PlayOneShot(typeSound);
            gameOverText.text += letter;
            yield return new WaitForSeconds(0.08f);
        }
    }
}
