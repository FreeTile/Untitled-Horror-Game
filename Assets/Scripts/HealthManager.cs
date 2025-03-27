using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class HealthManager : MonoBehaviour
{

    public Image healthImage;

    public Sprite fullHealthSprite;
    public Sprite mediumHealthSprite;
    public Sprite lowHealthSprite;

    public int health { get; private set; } = 3;
    

    public void DecreaseHealth()
    {
        Debug.Log(health);
        health -= 1;
        if (health <= 0)
        {
            GameManager.Instance.GameOver();
        }
        UpdateHealthSprite();
    }

    public void IncreaseHealth()
    {
        health += 1;
        if (health > 3)
        {
            health = 3;
        }
        UpdateHealthSprite();
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
}
