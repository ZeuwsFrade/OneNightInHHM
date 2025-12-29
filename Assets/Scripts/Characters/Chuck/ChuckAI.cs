using UnityEngine;

public class ChuckAI : MonoBehaviour
{
    public Transform playerTransform;
    private UnityEngine.AI.NavMeshAgent agent;
    public AudioSource yellingAudioSource;

    [Header("Настройки")]
    public float sightRange = 10f;
    public float fieldOfView = 90f;
    public AudioClip YellingSound;

    [Header("Преследование")]
    public float chaseTimeAfterLostSight = 5f;
    private float chaseTimer = 0f;
    private Vector3 lastKnownPlayerPosition;

    [Header("Патрулирование")]
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    public float waitTimeAtPoint = 2f;
    private float waitCounter;
    private bool isWaiting = false;

    [Header("Двери")]
    public float doorDetectionRange = 3f;
    public float doorInteractionAngle = 45f;
    public LayerMask doorLayer = 6;
    private bool hasDoorInFront = false;
    private DoorController nearestDoor;

    private bool isChasing = false;
    private bool hasStartedChase = false;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (yellingAudioSource != null && YellingSound != null)
        {
            yellingAudioSource.clip = YellingSound;
            yellingAudioSource.loop = false;
        }

        if (doorLayer.value == 0)
        {
            doorLayer = LayerMask.GetMask("Interactables");
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        bool canSeePlayer = IsPlayerInSight(distanceToPlayer);

        if (canSeePlayer)
        {
            if (!isChasing)
            {
                StartChasing();
            }

            lastKnownPlayerPosition = playerTransform.position;
            chaseTimer = chaseTimeAfterLostSight;

            agent.SetDestination(playerTransform.position);
        }
        else if (isChasing)
        {
            chaseTimer -= Time.deltaTime;

            if (chaseTimer > 0)
            {
                agent.SetDestination(lastKnownPlayerPosition);

                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    SearchAroundLastPosition();
                }
            }
            else
            {
                StopChasing();
                Patrol();
            }
        }
        else
        {
            Patrol();
        }

        if (isChasing)
        {
            CheckForDoors();
        }
    }

    void CheckForDoors()
    {
        hasDoorInFront = false;
        nearestDoor = null;

        Collider[] doorsInRange = Physics.OverlapSphere(transform.position, doorDetectionRange, doorLayer);

        if (doorsInRange.Length > 0)
        {
            DoorController closestDoor = null;
            float closestDistance = float.MaxValue;
            Vector3 closestDirection = Vector3.zero;

            foreach (Collider doorCollider in doorsInRange)
            {
                DoorController door = doorCollider.GetComponent<DoorController>();
                if (door != null && !door.isOpen)
                {
                    Vector3 toDoor = doorCollider.transform.position - transform.position;
                    float distance = toDoor.magnitude;

                    float angleToDoor = Vector3.Angle(agent.velocity.normalized, toDoor.normalized);

                    if (angleToDoor < doorInteractionAngle && distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestDoor = door;
                        closestDirection = toDoor;
                    }
                }
            }

            if (closestDoor != null)
            {
                hasDoorInFront = true;
                nearestDoor = closestDoor;

                if (!nearestDoor.isOpen)
                {
                    OpenDoor(nearestDoor);
                }
            }
        }
    }

    void OpenDoor(DoorController door)
    {
        if (door != null && !door.isOpen)
        {
            Vector3 toDoor = door.transform.position - transform.position;
            Vector3 doorForward = door.transform.forward;

            float dotProduct = Vector3.Dot(toDoor.normalized, doorForward);

            door.OpenDoor(true);
        }
    }

    void StartChasing()
    {
        isChasing = true;
        hasStartedChase = true;
        chaseTimer = chaseTimeAfterLostSight;

        if (yellingAudioSource != null && YellingSound != null)
        {
            if (!yellingAudioSource.isPlaying)
            {
                yellingAudioSource.Play();
            }
        }

    }

    void StopChasing()
    {
        isChasing = false;
        hasStartedChase = false;
        chaseTimer = 0f;

        if (yellingAudioSource != null && yellingAudioSource.isPlaying)
        {
            yellingAudioSource.Stop();
        }
    }

    void SearchAroundLastPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 3f;
        randomDirection.y = 0;
        Vector3 searchPosition = lastKnownPlayerPosition + randomDirection;

        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(searchPosition, out hit, 3f, UnityEngine.AI.NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    bool IsPlayerInSight(float distance)
    {
        if (distance > sightRange) return false;

        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, directionToPlayer) > fieldOfView / 2)
            return false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    void Patrol()
    {
        if (!isWaiting)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);

            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                isWaiting = true;
                waitCounter = waitTimeAtPoint;
            }
        }
        else
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                isWaiting = false;
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInteraction playerDeath= collision.gameObject.GetComponent<PlayerInteraction>();
            if (playerDeath != null)
            {
                playerDeath.Death();
            }
        }
    }
}