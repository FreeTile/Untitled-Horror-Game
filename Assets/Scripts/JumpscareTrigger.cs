using UnityEngine;

public class JumpScareTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.TriggerJumpScare();
        }
    }
}
