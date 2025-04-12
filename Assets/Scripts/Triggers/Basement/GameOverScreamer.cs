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
    public Image fadeImage;              // UI Image для затемнения экрана
    public TMP_Text gameOverText;            // UI Text для вывода сообщения

    private Animator monsterAnimator;

    void Start()
    {
        // Инициализация объекта монстра
        if (monster != null)
        {
            monsterAnimator = monster.GetComponent<Animator>();
            monster.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Monster не задан в инспекторе!");
        }

        // Инициализация UI: делаем картинку прозрачной и скрываем текст
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
            GameManager.Instance.switchControlSystem();
            // Отключаем ввод через GameInputHandler, чтобы игрок не мог нажимать кнопки
            //if (GameInputHandler.Instance != null)
            //{
            //    Debug.Log("Zashel");
            //    GameInputHandler.Instance.enabled = false;
            //}

            // Запускаем корутину для показа jumpscare и перехода в меню
            //StartCoroutine(PlayAttackAndGameOver());
            //SceneTransition.SwitchToScene("Menu");
            SceneManager.LoadScene("Menu");
        }
    }

    IEnumerator PlayAttackAndGameOver()
    {
        // Активируем монстра и проигрываем анимацию атаки
        monster.SetActive(true);
        monsterAnimator.Play(attackAnimation);

        // Ожидаем окончания анимации (замените 2.0f на реальное время анимации или используйте Animation Event)
        yield return new WaitForSeconds(2.0f);

        // Эффект затемнения экрана
        float fadeDuration = 1.0f;
        float elapsedTime = 0f;
        Color initialColor = fadeImage.color; // изначально alpha = 0
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // Интерполируем alpha от 0 до 1
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            Color newColor = initialColor;
            newColor.a = alpha;
            fadeImage.color = newColor;
            yield return null;
        }

        // Показываем сообщение о завершении игры
        gameOverText.text = "Demo is Over\r\nThanks for playing!";
        gameOverText.gameObject.SetActive(true);

        // Ожидаем 5 секунд перед переходом в меню
        yield return new WaitForSeconds(5f);

        // Используем вашу функцию для сцен транзишн, чтобы перейти в меню
        SceneTransition.SwitchToScene("Menu");
    }
}
