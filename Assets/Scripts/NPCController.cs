using UnityEngine;
using UnityEngine.AI;
using static ActManager;

public class NPCController : MonoBehaviour
{
    public Transform startPoint;  // Стартовая точка
    public Transform targetPoint; // Целевая точка
    public Transform exitPoint;   // Точка выхода

    private NavMeshAgent agent;
    private bool isMovingToTarget = false;
    private bool isMovingToExit = false;
    private Animator animator;
    private int animIDSpeed;



    public void SetStateInProgress()
    {
        ActManager.Instance.SetActState(ActManager.ActState.ActInProgress);
    }
    public void SetStateDone()
    {
        ActManager.Instance.SetActState(ActManager.ActState.ActDone);
    }
    public void SetStateEnd()
    {
        ActManager.Instance.SetActState(ActManager.ActState.ActEnd);

    }

    void Awake()
    {
        startPoint = GameObject.Find("Start").GetComponent<Transform>();
        targetPoint = GameObject.Find("Target").GetComponent<Transform>();
        exitPoint = GameObject.Find("Exit").GetComponent<Transform>();



        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animIDSpeed = Animator.StringToHash("Speed");
        ActManager.Instance.OnActStateChanged += OnActStateChanged;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(startPoint.position);
        if (ActManager.Instance.CurrentState == ActManager.ActState.ActStart || ActManager.Instance.CurrentState == ActManager.ActState.ActDone)
        {
            MoveToTarget();
        }

        if (ActManager.Instance.CurrentState == ActManager.ActState.ActEnd)
        {
            MoveToExit();
        }

        AssignAnimationIDs();
    }

    private void AssignAnimationIDs()
    {
    }

    void Update()
    {
        if (isMovingToTarget && Vector3.Distance(transform.position, targetPoint.position) < 0.5f)
        {
            isMovingToTarget = false;
        }

        if (isMovingToExit && Vector3.Distance(transform.position, exitPoint.position) < 0.5f)
        {
            isMovingToExit = false;
        }

        float speed = agent.velocity.magnitude;
        animator.SetFloat(animIDSpeed, speed);
    }

    public void MoveToTarget()
    {
        if (!isMovingToTarget)
        {
            agent.SetDestination(targetPoint.position);
            isMovingToTarget = true;
            isMovingToExit = false;
        }
    }

    public void SetActProgress()
    {
        ActManager.Instance.SetActState(ActManager.ActState.ActInProgress);
    }

    public void MoveToExit()
    {
        if (!isMovingToExit)
        {
            agent.SetDestination(exitPoint.position);
            isMovingToExit = true;
            isMovingToTarget = false;
        }
    }

    void OnDestroy()
    {
        if (ActManager.Instance != null)
        {
            ActManager.Instance.OnActStateChanged -= OnActStateChanged;
        }
    }

    private void OnActStateChanged(ActManager.ActState state)
    {
        switch (state)
        {
            case ActManager.ActState.ActStart:
                Debug.Log("Act started. NPC should start talking.");
                MoveToTarget();
                break;
            case ActManager.ActState.ActEnd:
                Debug.Log("Act ended. NPC should talk about the completed task.");
                MoveToExit();
                break;
            case ActManager.ActState.ActDone:
                MoveToTarget();
                break;
        }
    }
}