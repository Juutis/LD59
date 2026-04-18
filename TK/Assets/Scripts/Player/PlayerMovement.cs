using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform aimTarget;

    private float speed = 3f;
    private InputAction moveAction;
    private Vector2 dir;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        dir = moveAction.ReadValue<Vector2>();

        Vector3 targetDir = aimTarget.position - transform.position;
        float angle = Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new (targetDir.x, targetDir.z));
        transform.Rotate(Vector3.down, angle);
    }

    private void FixedUpdate()
    {
        Vector2 vel = dir * speed;
        rb.linearVelocity = new Vector3(vel.x, 0f, vel.y);
    }
}
