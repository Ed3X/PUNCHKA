using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostattackBehaviour : BaseBehaviour
{
    float cooldownTime = 1.0f;
    private float startTime;

    private void OnEnable()
    {
        startTime = Time.time;
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Verificar si el enemigo está muerto
        EnemyHealthSystem enemyHealth = animator.GetComponent<EnemyHealthSystem>();
        if (enemyHealth != null && enemyHealth.IsDead)
        {
            // Detener el comportamiento de postataque si el enemigo está muerto
            return;
        }

        bool isPlayerClose = CheckPlayer2(animator.transform);
        animator.SetBool("IsPlayerClose", isPlayerClose);
        bool isReachable = CheckPlayer3(animator.transform);
        animator.SetBool("IsAttacking", isReachable);

        Hold(animator.transform);
    }


    private void Hold(Transform transform)
    {
        while (Time.time - startTime < cooldownTime)
        {
            transform.position = transform.position;
        }
    }
}
