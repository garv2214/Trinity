using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack, Idle }
    public State state = State.Patrol;

    public Transform[] patrolPoints;
    int currentPoint = 0;

    public float chaseDistance = 10f;
    public float attackDistance = 2.2f;
    public int attackDamage = 10;
    public float attackInterval = 1.2f;

    NavMeshAgent agent;
    Transform player;
    Health health;
    float lastAttackTime = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
    }

    void Update()
    {
        if (health == null || health.currentHealth <= 0) return;

        float distToPlayer = player ? Vector3.Distance(transform.position, player.position) : Mathf.Infinity;

        switch (state)
        {
            case State.Patrol:
                PatrolUpdate(distToPlayer);
                break;
            case State.Chase:
                ChaseUpdate(distToPlayer);
                break;
            case State.Attack:
                AttackUpdate(distToPlayer);
                break;
            case State.Idle:
                agent.isStopped = true;
                break;
        }
    }

    void PatrolUpdate(float distToPlayer)
    {
        if (patrolPoints.Length == 0) return;
        agent.isStopped = false;
        agent.speed = 2f;
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }

        if (distToPlayer <= chaseDistance)
        {
            state = State.Chase;
        }
    }

    void ChaseUpdate(float distToPlayer)
    {
        if (player == null) { state = State.Patrol; return; }
        agent.isStopped = false;
        agent.speed = 4f;
        agent.SetDestination(player.position);

        if (distToPlayer <= attackDistance)
        {
            state = State.Attack;
            agent.isStopped = true;
        }
        else if (distToPlayer > chaseDistance * 1.2f) // lost
        {
            state = State.Patrol;
        }
    }

    void AttackUpdate(float distToPlayer)
    {
        if (player == null) { state = State.Patrol; return; }

        transform.LookAt(player);
        if (distToPlayer > attackDistance + 0.3f)
        {
            state = State.Chase;
            agent.isStopped = false;
            return;
        }

        if (Time.time - lastAttackTime >= attackInterval)
        {
            TryDealDamage();
            lastAttackTime = Time.time;
        }
    }

    void TryDealDamage()
    {
        if (player == null) return;
        Health ph = player.GetComponent<Health>();
        if (ph != null) ph.TakeDamage(attackDamage);
        // TODO: play attack animation
    }
}
