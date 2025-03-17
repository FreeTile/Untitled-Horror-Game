using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Image healthImage;

    public Sprite fullHealthSprite;
    public Sprite mediumHealthSprite;
    public Sprite lowHealthSprite;

    private int health = 3;

    void Start()
    {
        UpdateHealthSprite();
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage();
        }
    }

    void TakeDamage() //Implemented TakeDamage() function, please use that to update the Heart on the HUD
    {
        health--;
        UpdateHealthSprite();

        if (health <= 0)
        {
            GameOver();
        }
    }

    void UpdateHealthSprite()
    {
        switch (health)
        {
            case 3:
                healthImage.sprite = fullHealthSprite;
                break;
            case 2:
                healthImage.sprite = mediumHealthSprite;
                break;
            case 1:
                healthImage.sprite = lowHealthSprite;
                break;
            default:
                healthImage.sprite = null;
                break;
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        Debug.Log("Game Over");
    }
}
