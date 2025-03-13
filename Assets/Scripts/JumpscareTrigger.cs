using UnityEngine;

public class JumpScareTrigger : MonoBehaviour
{
    // Если нужно, можно добавить проверку по тегу, чтобы срабатывать только при столкновении с игроком
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.TriggerJumpScare();
        }
    }
}
