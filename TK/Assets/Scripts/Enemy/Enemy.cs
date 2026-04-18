using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
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
    private float TARGET_DIRECTION_MARGIN = 0.1f;

    private Rigidbody rb;

    private float movementSpeed = 5.0f;
    private float turnSpeed = 90.0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        checkPlayerVisibility();
    }

    private void patrol()
    {
        checkPlayerVisibility();
    }

    private void attack()
    {
        if (canSeePlayer())
        {
            TargetLocation = player.transform.position;
            TargetDirection = player.transform.position - transform.position;
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
        return true;
    }

    private void handleMoving()
    {
        Debug.Log("moveTarget=" + moveTarget);
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
        Debug.Log("New path aquired! length=" + newPath.corners.Length);
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
}

