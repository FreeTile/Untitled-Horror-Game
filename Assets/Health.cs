using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Image fullHealthImage;
    public Image mediumHealthImage;
    public Image lowHealthImage;

    private int health = 3;

    void Start()
    {
        UpdateHealthImage();
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        health--;
        UpdateHealthImage();

        if (health <= 0)
        {
            GameOver();
        }
    }

    void UpdateHealthImage()
    {
        fullHealthImage.gameObject.SetActive(health == 3);
        mediumHealthImage.gameObject.SetActive(health == 2);
        lowHealthImage.gameObject.SetActive(health == 1);
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        Debug.Log("Game Over");
    }
}
