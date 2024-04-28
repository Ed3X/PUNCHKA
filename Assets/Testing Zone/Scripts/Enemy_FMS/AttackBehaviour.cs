using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : BaseBehaviour
{
    public float attackRange = 1.0f;
    public float attackCooldown = 1.0f;
    public int Damage = 10;

    public float lastAttackTime = 0.0f;

    public string playerTag = "Player";
    private GameObject playerObject;

    public int AttackNumber = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        playerObject = GameObject.FindGameObjectWithTag(playerTag);
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Verificar si el enemigo está muerto
        EnemyHealthSystem enemyHealth = animator.GetComponent<EnemyHealthSystem>();
        if (enemyHealth != null && enemyHealth.IsDead)
        {
            // Detener el comportamiento de ataque si el enemigo está muerto
            return;
        }

        bool isPlayerClose = CheckPlayer2(animator.transform);
        animator.SetBool("IsPlayerClose", isPlayerClose);
        bool isReachable = CheckPlayer3(animator.transform);
        animator.SetBool("IsAttacking", isReachable);

        if (isPlayerClose && isReachable)
        {
            playerObject = GameObject.FindGameObjectWithTag(playerTag); // Find player object (consider alternatives)
            if (Time.time - lastAttackTime >= attackCooldown) // Check cooldown
            {
                AttackPlayer();
                lastAttackTime = Time.time; // Update last attack time
            }
        }
    }

    private void AttackPlayer()
    {
        if (playerObject != null)
        {
            Debug.Log("Attacks" + AttackNumber);
            playerObject.GetComponent<PlayerHealthSystem>().PlayerTakesDamage(Damage);

            AttackNumber++;
        }
        else
        {
            Debug.Log("Player doesn't exist");
        }
    }
}
