
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Ghost : MonoBehaviour
{
    private Animator Anim;
    [HideInInspector]
    public NavMeshAgent agent;

    // Cache hash values
    private static readonly int MoveState = Animator.StringToHash("Base Layer.move");
    private static readonly int AttackState = Animator.StringToHash("Base Layer.attack");
    private static readonly int IdleState = Animator.StringToHash("Base Layer.idle");

    [Header("Patrol Point")]
    [SerializeField] private Transform patrolStart;
    [SerializeField] private Transform patrolEnd;
    [SerializeField] private float patrolSpeed = 0.5f;
    private Vector3 currentTarget;

    [Header("Player Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 60f;
    [SerializeField] private float detectionAngle = 45f;
    [SerializeField] private float chaseSpeed = 1f;
    private Vector3 targetedPlayerPosition;

    [Header("Look Around")]
    [SerializeField] private float maxLookAroundAngle = 45f;
    [SerializeField] private float rotationSpeed = 2f;
    private Quaternion originalRotation;
    private bool isLookingAround = false;
    [SerializeField] private float lookAroundTimer = 5f;
    private float lookAroundTimeElapsed = 0f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1f;

    // Ghost states
    private enum GhostState { Patrolling, Chasing, Returning }
    private GhostState currentState;
    [HideInInspector]
    public bool isCheatModeActive = false;
    public Maze maze;
    public FadeScreen fadeScreen;
    public Transform playerCamera;

    private float stuckTimeout = 5f;
    private float stuckTimeElapsed = 0f;
    private Vector3 lastPosition;

    private void Start()
    {
        Anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // Set initial state and target
        currentState = GhostState.Patrolling;
        currentTarget = patrolStart.position;
        agent.speed = patrolSpeed;
    }

    private void Update()
    {
        if (isCheatModeActive)
        {
            StopGhost();
            return;
        }
        // Check for movement
        if (Vector3.Distance(transform.position, lastPosition) < 0.01f)
        {
            stuckTimeElapsed += Time.deltaTime;
            if (stuckTimeElapsed > stuckTimeout)
            {
                Debug.Log("Ghost is stuck, returning to patrol!");
                currentState = GhostState.Returning;
                agent.speed = patrolSpeed;
                stuckTimeElapsed = 0f;
            }
        }
        else
        {
            stuckTimeElapsed = 0f;
        }

        lastPosition = transform.position;

        switch (currentState)
        {
            case GhostState.Patrolling:
                Patrol();
                break;

            case GhostState.Chasing:
                ChasePlayer();
                break;

            case GhostState.Returning:
                ReturnToPatrol();
                break;
        }
    }

    //---------------------------------------------
    // Patrol between points
    //---------------------------------------------
    private void Patrol()
    {
        Anim.CrossFade(MoveState, 0.1f, 0, 0);

        // Set NavMeshAgent destination
        agent.SetDestination(currentTarget);

        // Switch target when reaching a patrol point
        if (Vector3.Distance(transform.position, currentTarget) < 0.5f)
        {
            currentTarget = (currentTarget == patrolStart.position) ? patrolEnd.position : patrolStart.position;
        }

        // Check if player is in sight
        if (IsPlayerInSight())
        {
            currentState = GhostState.Chasing;
            agent.speed = chaseSpeed;
        }
    }

    //---------------------------------------------
    // Chase the player
    //---------------------------------------------
    private void ChasePlayer()
    {
        Anim.CrossFade(MoveState, 0.1f, 0, 0);
        if (!isLookingAround)
        {
            // Validate the path before setting the destination
            NavMeshPath path = new();
            if(agent.CalculatePath(targetedPlayerPosition, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                // Set NavMeshAgent destination to the player's last known position
                agent.SetDestination(targetedPlayerPosition);
            }
            else
            {
                currentState = GhostState.Returning;
                agent.speed = patrolSpeed;
                return;
            }

            // Check if has reached the last known position
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                // Start looking around
                isLookingAround = true;
                lookAroundTimeElapsed = 0f;
                originalRotation = transform.rotation;
            }
        }
        else
        {
            LookAround();
            if (IsPlayerInSight())
            {
                isLookingAround = false;
            }
            else if (lookAroundTimeElapsed > lookAroundTimer)
            {
                isLookingAround = false;
                currentState = GhostState.Returning;
                agent.speed = patrolSpeed;
            }
        }

        // Check if the player is within attack range
        if (IsPlayerInAttackRange())
        {
            TriggerAttack();
        }
    }

    private Vector3 GetClosestNavMeshPosition(Vector3 targetPosition)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 2.0f, NavMesh.AllAreas))
        {
            Vector3 offset = (transform.position - hit.position).normalized * 0.5f; // Offset by 0.5 units
            return hit.position + offset;
        }
        return transform.position; // Fallback to the current position
    }

    private void LookAround()
    {
        lookAroundTimeElapsed += Time.deltaTime;
        float lookAroundAngle = Mathf.Sin(lookAroundTimeElapsed * Mathf.PI / lookAroundTimer) * maxLookAroundAngle;
        Quaternion targetRotation = Quaternion.Euler(0f, originalRotation.eulerAngles.y + lookAroundAngle, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private bool IsPlayerInAttackRange()
    {
        if (player == null) return false;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= attackRange;
    }

    private void TriggerAttack()
    {
        // Verify if the player is still in the attack range
        if (!IsPlayerInAttackRange())
            return;

        // Stop the ghost's movement
        agent.isStopped = true;

        // Move ghost in front of the player
        PositionInFrontOfPlayer();

        // Play the attack animation
        Anim.CrossFade(AttackState, 0.1f, 0);

        float attackAnimationDuration = GetAnimationClipLength("ghost_attack");
        // Back to start point
        Invoke(nameof(BackToStartPoint), attackAnimationDuration + 1f);
    }

    private void PositionInFrontOfPlayer()
    {
        if (player == null) return;

        Vector3 offset = playerCamera.forward * attackRange;
        Vector3 targetPosition = playerCamera.position + offset;

        // Move ghost to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
        Vector3 lookDirection = new Vector3(playerCamera.position.x, transform.position.y, playerCamera.position.z) - transform.position;
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    }

    // clipname = ghost_???
    private float GetAnimationClipLength(string clipName)
    {
        foreach (var clip in Anim.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }

        Debug.LogWarning($"Animation clip '{clipName}' not found.");
        return 0f; // Return 0 if clip not found
    }

    public void BackToStartPoint()
    {
        fadeScreen.FadeIn();
        player.position = new Vector3(maze.startPosition.x, maze.startPosition.y + 2, maze.startPosition.z);
        agent.isStopped = false;
        transform.position = patrolStart.position;
        currentState = GhostState.Patrolling;
        currentTarget = patrolStart.position;
        agent.speed = patrolSpeed;
    }

    //---------------------------------------------
    // Return to patrolling
    //---------------------------------------------
    private void ReturnToPatrol()
    {
        Anim.CrossFade(MoveState, 0.1f, 0, 0);

        // Set NavMeshAgent destination to the nearest patrol point
        agent.SetDestination(currentTarget);

        // If near a patrol point, resume patrolling
        if (Vector3.Distance(transform.position, currentTarget) < 0.5f)
        {
            currentState = GhostState.Patrolling;
        }
    }

    //---------------------------------------------
    // Check if the player is in sight
    //---------------------------------------------
    private bool IsPlayerInSight()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Check angle
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle > detectionAngle)
            return false;

        // Optional: Check if there's an obstacle between the ghost and player
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, detectionRange))
        {
            if (hit.collider.gameObject == player.gameObject)
            {
                targetedPlayerPosition = GetClosestNavMeshPosition(player.position);
                return true;
            }
        }

        return false;
    }

    //---------------------------------------------
    // Stop ghost movement
    //---------------------------------------------
    private void StopGhost()
    {
        agent.isStopped = true;
        transform.position = patrolStart.position;
        transform.rotation = Quaternion.LookRotation(patrolStart.forward, Vector3.up);
        Anim.CrossFade(IdleState, 0.1f, 0, 0);
    }
}
