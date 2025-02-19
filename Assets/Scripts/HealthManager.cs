using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HealthManager : MonoBehaviour
{
    public int health { get; private set; }

    public void DecreaseHealth()
    {
        health -= 1;
        if (health <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void IncreaseHealth()
    {
        health += 1;
        if (health > 3)
        {
            health = 3;
        }
    }
}
