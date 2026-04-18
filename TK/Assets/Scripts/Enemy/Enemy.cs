using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private BulletTrail bulletTrailPrefab;

    private GameObject player;
    private EnemyState state;

    private Vector3 TargetLocation;
    private Vector3 TargetDirection;

    private Vector3 myPosition
    {
        get
        {
            return transform.position;
        }
    }

    private bool isAlive = true;

    private float TARGET_LOCATION_MARGIN = 0.1f;

    private Rigidbody rb;

    private float movementSpeed = 5.0f;
    private float turnSpeed = 360.0f;

    private int visionCheckLayers;
    private float visionRange = 10;
    private float visionAngle = 70;
    private float smellDistance = 1.5f;

    private float attackRange = 3.0f;
    private int minBurst = 8;
    private int maxBurst = 10;
    private float aimingDuration = 0.6f;
    private float trackingDuration = 0.4f;
    private float backSwingDuration = 0.8f;
    private float minTimeBetweenBursts = 1.0f;
    private float maxTimeBetweenBursts = 1.5f;
    private float fireRate = 10;
    private float accuracyDegrees = 20;

    private int burstRemaining = 0;
    private AttackState attackState = AttackState.PURSUE;
    private float aimingTimer;
    private float trackingTimer;
    private float shootTimer;
    private float backSwingTimer;
    private float burstTimer;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        visionCheckLayers = LayerMask.GetMask("World", "Player");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        UpdatePathing();
    }

    // Update is called once per frame
    void Update()
    {
        handlePathing();
        handleRotation();
        switch(state)
        {
            case EnemyState.IDLE:
                idle();
                break;
            case EnemyState.PATROL:
                patrol();
                break;
            case EnemyState.ATTACK:
                attack();
                break;
        }
    }

    void FixedUpdate()
    {
        if (isAlive)
        {
            handleMoving();
        }
    }

    private void idle()
    {
        TargetLocation = myPosition;
        checkPlayerVisibility();
    }

    private void patrol()
    {
        checkPlayerVisibility();
    }

    private void attack()
    {
        var hasVision = canSeePlayer();
        var playerDir = player.transform.position - myPosition;
        if (hasVision)
        {
            TargetLocation = player.transform.position;
        }

        switch (attackState)
        {
            case AttackState.PREPARE:
                moveTarget = myPosition;
                if (trackingTimer > Time.time)
                {
                    TargetDirection = playerDir;
                }
                if (aimingTimer < Time.time)
                {
                    attackState = AttackState.SHOOT;
                    burstRemaining = Random.Range(minBurst, maxBurst + 1);
                }
                break;
            case AttackState.SHOOT:
                moveTarget = myPosition;
                if (burstRemaining > 0)
                {
                    if (shootTimer < Time.time)
                    {
                        shoot();
                        shootTimer = Time.time + 1.0f / fireRate;
                        burstRemaining--;
                    }
                }
                else
                {
                    attackState = AttackState.BACKSWING;
                    backSwingTimer = Time.time + backSwingDuration;
                }
                break;
            case AttackState.BACKSWING:
                moveTarget = myPosition;
                if (backSwingTimer < Time.time)
                {
                    attackState = AttackState.PURSUE;
                    burstTimer = Time.time + Random.Range(minTimeBetweenBursts, maxTimeBetweenBursts);
                }
                break;
            case AttackState.PURSUE:
                if (hasVision)
                {
                    TargetDirection = playerDir;
                    if (Vector3.Distance(myPosition, player.transform.position) < attackRange)
                    {
                        moveTarget = myPosition;
                        if (burstTimer < Time.time)
                        {
                            attackState = AttackState.PREPARE;
                            aimingTimer = Time.time + aimingDuration;
                            trackingTimer = Time.time + trackingDuration;
                        }
                    }
                }
                else
                {
                    TargetDirection = TargetLocation - myPosition;
                }
                break;
        }
    }

    private void checkPlayerVisibility()
    {
        if (canSeePlayer())
        {
            state = EnemyState.ATTACK;
        }
    }

    private bool canSeePlayer()
    {
        var distanceToPlayer = Vector3.Distance(myPosition, player.transform.position);
        var dir = player.transform.position - myPosition;

        // ignore vision if player is close enough
        if (distanceToPlayer > smellDistance)
        {
            if (distanceToPlayer > visionRange)
            {
                return false;
            }
            if (Vector3.Angle(transform.forward, dir) > visionAngle)
            {
                return false;
            }
        }

        if (Physics.Raycast(myPosition, dir, out var hit, 1000.0f, visionCheckLayers))
        {
            if (hit.collider.gameObject == player)
            {
                return true;
            }
        }
        return false;
    }

    private void handleMoving()
    {
        if (Vector3.Distance(myPosition, moveTarget) > TARGET_LOCATION_MARGIN)
        {
            rb.linearVelocity = (moveTarget - myPosition).normalized * movementSpeed;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void handleRotation()
    {
        if (!isAlive)
        {
            return;
        }

        var rotation = Vector3.SignedAngle(transform.forward, TargetDirection, Vector3.up);
        var maxRotationPerTick = Time.deltaTime * turnSpeed;
        var rotationPerTick = Mathf.Clamp(rotation, -maxRotationPerTick, maxRotationPerTick);
        transform.Rotate(transform.up, rotationPerTick);
    }

    private void shoot()
    {
        var dispersion = Random.Range(-accuracyDegrees, accuracyDegrees);
        var dispersionQuat = Quaternion.AngleAxis(dispersion, Vector3.up);
        var dir = dispersionQuat * TargetDirection;
        var trailEnd = myPosition + dir.normalized * attackRange * 4;
        if (Physics.Raycast(myPosition, dir, out var hit, attackRange * 4, visionCheckLayers))
        {
            trailEnd = hit.point;
            if (hit.collider.gameObject == player)
            {
            }
        }
        var bulletTrail = Instantiate(bulletTrailPrefab);
        bulletTrail.Init(myPosition, trailEnd);
    }

    
    // *************************
    // PATHFINDING
    // *************************
    private NavMeshPath path;
    private int cornerIndex;

    private float pathingInterval = 0.2f;
    private float desiredRange = 0.1f;
    private Vector3 moveTarget;
    
    private void handlePathing()
    {
        if (HasPath())
        {
            if (IsLastCorner() && distanceToNextCorner() < desiredRange)
            {
                moveTarget = transform.position;
            }
            else
            {
                moveTarget = getNextCorner();
            }
        } else
        {
            moveTarget = transform.position;
        }
    }

    private Vector3 getNextCorner()
    {
        while (!IsLastCorner() && distanceToNextCorner() < 0.1f)
        {
            cornerIndex++;
        }
        return path.corners[cornerIndex];
    }

    private float distanceToNextCorner()
    {
        return Vector3.Distance(path.corners[cornerIndex], transform.position);
    }

    private bool IsLastCorner()
    {
        return cornerIndex >= path.corners.Length - 1;
    }

    private void UpdatePathing()
    {
        path = GetPathTo(TargetLocation);
        cornerIndex = 0;
        Invoke("UpdatePathing", pathingInterval);
    }

    private bool HasPath()
    {
        if (path == null)
        {
            return false;
        }
        return path.corners.Length > 0;
    }

    private NavMeshPath GetPathTo(Vector3 target)
    {
        NavMeshPath newPath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, newPath);
        return newPath;
    }

    private void debugPathing()
    {
        Vector3 prev = transform.position;
        for(int i = 0; i<path.corners.Length; i++)
        {
            Debug.DrawLine(prev, path.corners[i], Color.red);
            prev = path.corners[i];
        }
        Debug.DrawLine(transform.position, path.corners[cornerIndex], Color.green);
    }

    
    private enum EnemyState
    {
        IDLE,
        PATROL,
        ATTACK
    }

    private enum AttackState
    {
        PREPARE,
        SHOOT,
        BACKSWING,
        PURSUE
    }
}

