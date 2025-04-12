using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    [SerializeField] 
    private HealthManager healthManager;
    [SerializeField]
    public MonsterAI monsterAI;
    [SerializeField]
    private Animator animator;

    private bool invulnerable = false;
    private float damageDelay = 3f;
    void OnTriggerEnter(Collider other)
    {
        if (invulnerable) return;
        if (other.gameObject.tag == "Player" && invulnerable == false)
        {
            monsterAI.playerDamaged = true;
            attackPlayer();
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
        animator.Play("Attack");
        // Wait for the specified amount of time
        yield return new WaitForSeconds(damageDelay);

        // Set the invulnerable flag to false
        invulnerable = false;
        monsterAI.playerDamaged = false;
    }
}
