using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private BulletTrail bulletTrailPrefab;
    [SerializeField]
    private GameObject bodyPrefab;

    [SerializeField]
    private EnemyConfig config;

    [SerializeField]
    private Animator anim;


    private GameObject player;
    private EnemyState state = EnemyState.PATROL;

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

    private int visionCheckLayers;

    private int burstRemaining = 0;
    private AttackState attackState = AttackState.PURSUE;
    private float aimingTimer;
    private float trackingTimer;
    private float shootTimer;
    private float backSwingTimer;
    private float burstTimer;

    private float hurtTimer = 0;

    private Vector3 patrolTarget;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        visionCheckLayers = LayerMask.GetMask("World", "Player");
        transform.up = Vector3.up;
        transform.Rotate(Vector3.up, Random.Range(0f, 360f));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AntennaManager.Instance.Enemies.Add(this);
        player = GameObject.FindGameObjectWithTag("Player");
        UpdatePathing();
        GetComponent<CharacterHealth>().InitHealth(config.Health, Death, Hurt);
        ResetPatrol();
    }

    private void ResetPatrol()
    {
        var maxOffset = 5.0f;
        var offSet = new Vector3(Random.Range(-maxOffset, maxOffset), 0, Random.Range(-maxOffset, maxOffset));
        patrolTarget = transform.position + offSet;
        Invoke("ResetPatrol", Random.Range(3.0f, 8.0f));
    }

    private void Death()
    {
        AntennaManager.Instance.Enemies.Remove(this);
        var body = Instantiate(bodyPrefab);
        body.transform.position = transform.position;
        body.transform.rotation = transform.rotation;
        Destroy(gameObject);
    }

    public void Hurt()
    {
        hurtTimer = Time.time + 1.0f;
        AlertNearbyEnemies();
    }

    public void TriggerAlert()
    {
        hurtTimer = Time.time + 1.0f;
    }

    private void AlertNearbyEnemies()
    {
        AntennaManager.Instance.Enemies.Where(it => Vector3.Distance(transform.position, it.transform.position) < 3.0f).ToList().ForEach(it => it.TriggerAlert());
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
        anim.Play(config.idleAnimation);
        TargetLocation = myPosition;
        checkPlayerVisibility();
    }

    private void patrol()
    {
        TargetLocation = patrolTarget;
        TargetDirection = patrolTarget - transform.position;
        anim.Play(config.idleAnimation);
        checkPlayerVisibility();
    }

    private void attack()
    {
        var hasVision = true;
        var playerDir = player.transform.position - myPosition;
        if (hasVision)
        {
            TargetLocation = player.transform.position;
        }

        switch (attackState)
        {
            case AttackState.PREPARE:
                anim.Play(config.aimAnimation);
                moveTarget = myPosition;
                if (trackingTimer > Time.time)
                {
                    TargetDirection = playerDir;
                }
                if (aimingTimer < Time.time)
                {
                    attackState = AttackState.SHOOT;
                    burstRemaining = Random.Range(config.MinBurst, config.MaxBurst + 1);
                }
                break;
            case AttackState.SHOOT:
                anim.Play(config.attackAnimation);
                moveTarget = myPosition;
                if (burstRemaining > 0)
                {
                    if (shootTimer < Time.time)
                    {
                        shoot();
                        shootTimer = Time.time + 1.0f / config.FireRate;
                        burstRemaining--;
                        AlertNearbyEnemies();
                    }
                }
                else
                {
                    attackState = AttackState.BACKSWING;
                    backSwingTimer = Time.time + config.BackSwingDuration;
                }
                break;
            case AttackState.BACKSWING:
                anim.Play(config.attackAnimation);
                moveTarget = myPosition;
                if (backSwingTimer < Time.time)
                {
                    attackState = AttackState.PURSUE;
                    burstTimer = Time.time + Random.Range(config.MinTimeBetweenBursts, config.MaxTimeBetweenBursts);
                }
                break;
            case AttackState.PURSUE:
                anim.Play(config.idleAnimation);
                if (hasVision)
                {
                    TargetDirection = playerDir;
                    if (Vector3.Distance(myPosition, player.transform.position) < config.AttackRange)
                    {
                        moveTarget = myPosition;
                        if (burstTimer < Time.time)
                        {
                            attackState = AttackState.PREPARE;
                            aimingTimer = Time.time + config.AimingDuration;
                            trackingTimer = Time.time + config.TrackingDuration;
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
            AlertNearbyEnemies();
        }
    }

    private bool canSeePlayer()
    {
        if (hurtTimer > Time.time)
        {
            return true;
        }
        
        var distanceToPlayer = Vector3.Distance(myPosition, player.transform.position);
        var dir = player.transform.position - myPosition;

        // ignore vision if player is close enough
        if (distanceToPlayer > config.SmellDistance)
        {
            if (distanceToPlayer > config.VisionRange)
            {
                return false;
            }
            if (Vector3.Angle(transform.forward, dir) > config.VisionAngle)
            {
                return false;
            }
        }

        if (Physics.Raycast(myPosition + Vector3.up * 0.1f, dir, out var hit, 1000.0f, visionCheckLayers))
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
            rb.linearVelocity = (moveTarget - myPosition).normalized * config.Speed;
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
        var maxRotationPerTick = Time.deltaTime * config.TurnSpeed;
        var rotationPerTick = Mathf.Clamp(rotation, -maxRotationPerTick, maxRotationPerTick);
        transform.Rotate(transform.up, rotationPerTick);
    }

    private void shoot()
    {
        var dispersion = Random.Range(-config.AccuracyVariationDegrees, config.AccuracyVariationDegrees);
        var dispersionQuat = Quaternion.AngleAxis(dispersion, Vector3.up);
        var dir = dispersionQuat * TargetDirection;
        var projectileRange = config.AttackRange + 2;
        if (config.melee)
        {
            projectileRange = config.AttackRange + 0.5f;
        }
        var trailEnd = myPosition + dir.normalized * projectileRange;
        if (Physics.Raycast(myPosition + 0.1f * Vector3.up, dir, out var hit, projectileRange, visionCheckLayers))
        {
            trailEnd = hit.point;
            if (hit.collider.gameObject == player)
            {
                var health = hit.collider.GetComponent<CharacterHealth>();
                health.Hurt(1, dir);
            }
        }
        if (!config.melee)
        {
            var bulletTrail = Instantiate(bulletTrailPrefab);
            bulletTrail.Init(myPosition, trailEnd);
        }
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

