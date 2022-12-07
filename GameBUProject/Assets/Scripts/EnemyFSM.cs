using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Dead,
        NoState
    }

    public State currentState = State.Idle;



    private EnemyAni myAni;
    private EnemyParams myParams;

    private Transform playerObj;
    private PlayerParams playerParams;

    private float chaseDistance = 5f;
    private float attackDistance = 2.5f;
    private float reChaseDistance = 3f;
    private float rotAnglePerSecond = 360f;
    private float moveSpeed = 1.3f;
    private float attackDelay = 2f;
    private float attackTimer = 0f;

    public ParticleSystem hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        myAni = GetComponent<EnemyAni>();
        myParams = GetComponent<EnemyParams>();
        myParams.deadEvent.AddListener(CallDeadEvent);

        ChangeState(State.Idle, EnemyAni.IDLE);

        playerObj = GameObject.FindGameObjectWithTag("Player").transform;
        playerParams = playerObj.gameObject.GetComponent<PlayerParams>();

        hitEffect.Stop();
    }

    void CallDeadEvent()
    {
        ChangeState(State.Dead, EnemyAni.DIE);
        playerObj.gameObject.SendMessage("CurrentEnemyDead");
    }

    public void ShowHitEffect()
    {
        hitEffect.Play();
    }

    void ChangeState(State newState, string aniName)
    {
        if (currentState == newState)
            return;

        myAni.ChangeAni(aniName);
        currentState = newState;
    }

    void UpdateState()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Chase:
                ChaseState();
                break;
            case State.Attack:
                AttackState();
                break;
            case State.Dead:
                DeadState();
                break;
            case State.NoState:
                NoState();
                break;
            default:
                break;
        }
    }

    void IdleState()
    {
        if (GetaDistanceFromPlayer() < chaseDistance )
        {
            ChangeState(State.Chase, EnemyAni.WALK);
        }
    }
    void ChaseState()
    {
        if (GetaDistanceFromPlayer() < attackDistance)
        {
            ChangeState(State.Attack, EnemyAni.ATTACK);
        }
        else
        {
            TurnToDestination();
            MoveToDestination();
        }
    }
    void AttackState()
    {
        if (GetaDistanceFromPlayer() > reChaseDistance)
        {
            attackTimer = 0;
            ChangeState(State.Chase, EnemyAni.WALK);
        }
        else
        {
            if (attackTimer > attackDelay)
            {
                transform.LookAt(playerObj.position);
                myAni.ChangeAni(EnemyAni.ATTACK);

                attackTimer = 0f;
            }
            attackTimer += Time.deltaTime;
        }
    }
    void DeadState()
    {
        GetComponent<BoxCollider>().enabled = false;
    }
    void NoState()
    {

    }

    public void TurnToDestination()
    {
        Quaternion lookRotation = Quaternion.LookRotation(playerObj.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecond);
    }

    public void MoveToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerObj.position, moveSpeed * Time.deltaTime);
    }

    float GetaDistanceFromPlayer()
    {
        return Vector3.Distance(transform.position, playerObj.position);
    }



    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }
}
