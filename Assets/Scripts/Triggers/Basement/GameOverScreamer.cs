using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameOverScreamer : MonoBehaviour
{
    [Header("Настройка монстра")]
    public GameObject monster;
    public string attackAnimation = "Attack";

    [Header("Настройка UI")]
    public Image fadeImage;
    public TMP_Text gameOverText;

    private Animator monsterAnimator;

    void Start()
    {
        if (monster != null)
        {
            monsterAnimator = monster.GetComponent<Animator>();
            monster.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Monster не задан в инспекторе!");
        }

        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
        else
        {
            Debug.LogWarning("Fade Image не задан в инспекторе!");
        }

        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Game Over Text не задан в инспекторе!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.state = GameManager.State.GameOver;
            GameManager.Instance.TriggerJumpScare();
            GameManager.Instance.switchControlSystem();
            
            StartCoroutine(PlayAttackAndGameOver());
            
        }
    }

    IEnumerator PlayAttackAndGameOver()
    {
        monster.SetActive(true);
        monsterAnimator.Play(attackAnimation);

        yield return new WaitForSeconds(2.0f);

        float fadeDuration = 1.0f;
        float elapsedTime = 0f;
        Color initialColor = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            Color newColor = initialColor;
            newColor.a = alpha;
            fadeImage.color = newColor;
            yield return null;
        }

        gameOverText.text = "Demo is Over\r\nThanks for playing!";
        gameOverText.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        SceneTransition.SwitchToScene("Menu");
    }
}
