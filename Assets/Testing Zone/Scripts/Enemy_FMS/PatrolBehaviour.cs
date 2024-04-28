using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : BaseBehaviour
{
    float _timer;
    private float Speed = 1;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _timer = 0f;

        Vector3 rdmPointInPlane = new Vector3(Random.Range(-100, 100), animator.transform.position.y, Random.Range(-100, 100));

        animator.transform.LookAt(rdmPointInPlane);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Verificar si el enemigo está muerto
        EnemyHealthSystem enemyHealth = animator.GetComponent<EnemyHealthSystem>();
        if (enemyHealth != null && enemyHealth.IsDead)
        {
            // Detener el comportamiento de patrulla si el enemigo está muerto
            return;
        }

        // Check triggers
        bool isTimeUp = CheckTime();
        bool isPlayerClose = CheckPlayer(animator.transform);

        animator.SetBool("IsPatrolling", !isTimeUp);
        animator.SetBool("IsChasing", isPlayerClose);

        // Do stuff
        Move(animator.transform);
    }


    private void Move(Transform mySelf)
    {
        mySelf.Translate(mySelf.forward * Speed * Time.deltaTime);
    }

    private bool CheckTime()
    {
        _timer += Time.deltaTime;
        return _timer > 4;
    }
}
