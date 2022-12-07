using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    public enum State
    {
        Idle,
        Move,
        Attack,
        AttackWait,
        Dead
    }

    public State currentState = State.Idle;

    private PlayerAni myAni;
    private PlayerParams myParams;
    private EnemyParams curEnemyParams;

    private Vector3 curTargetPos;

    private GameObject curEnemy;

    private float rotAnglePerSecond = 360f;
    private float moveSpeed = 2f;
    private float attackDelay = 2f;
    private float attackTimer = 0f;
    private float attackDistance = 1.5f;
    private float chaseDistance = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        myAni = GetComponent<PlayerAni>();
        myParams = GetComponent<PlayerParams>();
        myParams.InitParams();

        ChangeState(State.Idle, PlayerAni.ANI_IDLE);

    }

    public void AttackCalculate()
    {
        if (curEnemy == null)
            return;
        int attackPower = myParams.GetRandomAttack();
        curEnemyParams.SetEnemyAttack(attackPower);
        curEnemy.GetComponent<EnemyFSM>().ShowHitEffect();
    }

    public void CurrentEnemyDead()
    {
        ChangeState(State.Idle, PlayerAni.ANI_IDLE);
    }

    void ChangeState(State newState, int aniNumber)
    {
        if (currentState == newState)
            return;

        myAni.ChangeAni(aniNumber);
        currentState = newState;
    }

    void UpdateState()
    {
        switch(currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Move:
                MoverState();
                break;
            case State.Attack:
                AttackState();
                break;
            case State.AttackWait:
                AttackWaitState();
                break;
            case State.Dead:
                DeatState();
                break;
            default:
                break;
        }
    }

    void IdleState()
    {

    }
    void MoverState()
    {
        TurnToDestination();
        MoveToDestination();
    }
    void AttackState()
    {
        attackTimer = 0;
        transform.LookAt(curTargetPos);
        ChangeState(State.AttackWait, PlayerAni.ANI_ATTACKIDLE);
    }
    void AttackWaitState()
    {
        if (attackTimer > attackDelay)
        {
            ChangeState(State.Attack, PlayerAni.ANI_ATTACK);
        }
        attackTimer += Time.deltaTime;
    }
    void DeatState()
    {

    }

    public void MoveTo(Vector3 targetPos)
    {
        if (currentState == State.Dead)
            return;

        curEnemy = null;
        curTargetPos = targetPos;
        ChangeState(State.Move, PlayerAni.ANI_WALK);
    }

    public void TurnToDestination()
    {
        Quaternion lookRotation = Quaternion.LookRotation(curTargetPos-transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecond);
    }

    public void MoveToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, curTargetPos, moveSpeed * Time.deltaTime);

        if (curEnemy == null)
        {
            if (transform.position == curTargetPos)
                ChangeState(State.Idle, PlayerAni.ANI_IDLE);
        }
        else if (Vector3.Distance(transform.position, curTargetPos) < attackDistance)
        {
            ChangeState(State.Attack, PlayerAni.ANI_ATTACK);
        }
    }

    public void AttackEnemy(GameObject enemy)
    {
        if (curEnemy != null && curEnemy == enemy)
            return;

        curEnemyParams = enemy.GetComponent<EnemyParams>();
        if (curEnemyParams.isDead == false)
        {
            curEnemy = enemy;
            curTargetPos = curEnemy.transform.position;
            ChangeState(State.Move, PlayerAni.ANI_WALK);
        }
        else
        {
            curEnemyParams = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }
}
