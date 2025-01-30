using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;   //vi du: ghost thuoc truong shooter, chung ta co the keo lop game ban sung cua minh vao serializeField la  MonoBehaviour enemyType
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttack = false;

    private bool canAttack = true;

    private enum State {
        Roaming,
        Attacking
    }

    private Vector2 roamPosition;
    private float timeRoaming = 0f;

    private State state;
    private EnemyPathFinding enemyPathFinding;

    private void Awake() {
        enemyPathFinding = GetComponent<EnemyPathFinding>();
        state = State.Roaming;
    }

    private void Start() {
        // StartCoroutine(RoamingRoutine());
        roamPosition = GetRoamingPosition();
    }

    private void Update() {
        MovementStateControl();
        if (enemyType == null) {
            return;
        }
    }

    private void MovementStateControl() {
        switch (state)
        {
            
            default:
            case State.Roaming:
                // roam
                Roaming();
                break;
            case State.Attacking:
                // attack
                Attacking();
                break;
        }
    }

    private void Roaming() {
        timeRoaming += Time.deltaTime;
        
        enemyPathFinding.MoveTo(roamPosition);

        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange) {
            state = State.Attacking;
        }

        if (timeRoaming > roamChangeDirFloat) {
            roamPosition = GetRoamingPosition();
        } 
    }

    private void Attacking() {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            state = State.Roaming;
        }

        if (attackRange != 0 && canAttack) 
        {
            canAttack = false;
            (enemyType as IEnemy).Attack();

            if (stopMovingWhileAttack)
            {
                enemyPathFinding.StopMoving();    // stop moving
            }
            else
            {
                // enemyPathFinding.MoveTo(roamPosition);
                enemyPathFinding.MoveTo(GetRoamingPosition());
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine() {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // private IEnumerator RoamingRoutine() {
    //     while(state == State.Roaming) 
    //     {
    //         Vector2 roamPosition = GetRoamingPosition();
    //         // Debug.Log(roamPosition);
    //         enemyPathfinding.MoveTo(roamPosition);
    //         // yield return new WaitForSeconds(2f);
    //         yield return new WaitForSeconds(roamChangeDirFloat);
    //     }
    // }

    private Vector2 GetRoamingPosition() {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
