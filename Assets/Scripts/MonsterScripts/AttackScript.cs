using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    [SerializeField] 
    private HealthManager healthManager;
    [SerializeField]
    public MonsterBasement monsterAI;
    [SerializeField]
    private Animator animator;

    private bool invulnerable = false;
    private float damageDelay = 2.5f;
    void OnTriggerEnter(Collider other)
    {
        if (invulnerable) return;
        if (other.gameObject.tag == "Player" && invulnerable == false)
        {
            monsterAI.playerDamaged = true;
            invulnerable = true;
            StartCoroutine(DamageDelay());
        }

    }

    //attack fucntion
    private void attackPlayer()
    {
        healthManager.DecreaseHealth();
    }
    private IEnumerator DamageDelay()
    {
        //Put animation to play here 
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.2f);
        attackPlayer();
        // Wait for the specified amount of time
        yield return new WaitForSeconds(damageDelay);
        // Set the invulnerable flag to false
        invulnerable = false;
        monsterAI.playerDamaged = false;
    }
}
