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

    public bool IdDead { get; private set; }

    NavMeshAgent agent;
    Animator animator;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

        }
    }
}


