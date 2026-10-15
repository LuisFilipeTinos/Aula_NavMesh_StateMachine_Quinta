using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    //Variáveis de controle de estado:
    [SerializeField] float patrolSpeed = 1f;
    [SerializeField] float chasingSpeed = 2.5f;
    [SerializeField] float detectionRange = 15f;
    [SerializeField] float loseRange = 20f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float attackCooldown = 1.5f;
    [SerializeField] float attackDuration = 0.8f;

    public bool IsDead { get; private set; }

    NavMeshAgent agent;
    Animator animator;

    State current;
    PatrolState patrol;
    ChaseState chase;
    AttackState attack;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        patrol = new PatrolState(this);
        chase = new ChaseState(this);
        attack = new AttackState(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeState(patrol);
    }

    // Update is called once per frame
    void Update()
    {
        current.Execute();
    }

    void ChangeState(State next)
    {
        current = next;
        current.Enter();
    }

    float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    abstract class State
    {
        public EnemyController ai;
        public State(EnemyController artificialIntelligence)
        {
            ai = artificialIntelligence;
        }

        public abstract void Enter();
        public abstract void Execute();
    }

    class PatrolState : State
    {
        Transform target;

        public PatrolState(EnemyController ai) : base(ai) { }

        public override void Enter()
        {
            ai.animator.SetBool("Chasing", false);
            ai.agent.isStopped = false;
            ai.agent.speed = ai.patrolSpeed;

            if (target == null)
            {
                target = ai.pointA;
            }

            ai.agent.SetDestination(target.position);
        }

        public override void Execute()
        {
            if (ai.DistanceToPlayer() <= ai.detectionRange)
            {
                ai.ChangeState(ai.chase);
            }

            if (!ai.agent.pathPending && ai.agent.remainingDistance < 0.5f)
            {
                if (target == ai.pointA)
                {
                    target = ai.pointB;
                }
                else
                {
                    target = ai.pointA;
                }

                ai.agent.SetDestination(target.position);
            }
        }
    }

    class ChaseState : State
    {
        public ChaseState(EnemyController ai) : base(ai) { }

        public override void Enter()
        {
            ai.animator.SetBool("Chasing", true);
            ai.agent.isStopped = false;
            ai.agent.speed = ai.chasingSpeed;
        }

        public override void Execute()
        {
            ai.agent.SetDestination(ai.player.position);

            float distance = ai.DistanceToPlayer();

            if (distance <= ai.attackRange)
            {
                ai.ChangeState(ai.attack);
            }
            else if (distance > ai.loseRange)
            {
                ai.ChangeState(ai.patrol);
            }
        }
    }

    class AttackState : State
    {
        float timer;

        public AttackState(EnemyController ai) : base(ai) { }

        public override void Enter()
        {
            ai.animator.SetBool("Chasing", false);
            ai.animator.SetTrigger("Attack");
            ai.agent.isStopped = true;
            ai.agent.velocity = Vector3.zero;
            timer = ai.attackCooldown;
        }

        public override void Execute()
        {
            timer += Time.deltaTime;

            if (timer < ai.attackCooldown)
                return;

            if (ai.DistanceToPlayer() > ai.attackRange)
            {
                ai.ChangeState(ai.chase);
                return;
            }

            Vector3 lookPosition = ai.player.position;
            lookPosition.y = ai.transform.position.y;
            ai.transform.LookAt(lookPosition);

            if (timer >= ai.attackCooldown)
            {
                ai.animator.ResetTrigger("Attack");
                ai.animator.SetTrigger("Attack");
                timer = 0;
            }
        }
    }  

    public void Death()
    {
        if (IsDead)
            return;

        IsDead = true;

        agent.enabled = false;

        foreach (Collider coll in GetComponentsInChildren<Collider>())
        {
            coll.enabled = false;
        }

        animator.SetTrigger("Death");

        enabled = false;
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}


