using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform aimTarget;
    [SerializeField]
    private PlayerConfig playerConfig;

    private float speed;
    private InputAction moveAction;
    private Vector2 dir;
    private Rigidbody rb;

    private CharacterHealth health;

    [SerializeField]
    private GameObject youDied;

    [SerializeField]
    private GameObject body;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");

        rb = GetComponent<Rigidbody>();
        speed = playerConfig.Speed;

        health = GetComponent<CharacterHealth>();
        health.InitHealth(playerConfig.Health, Death, null);
        GameManager.instance.SetPlayerHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        dir = moveAction.ReadValue<Vector2>();

        Vector3 pos = transform.position;
        Vector3 camPos = Camera.main.transform.position;
        Camera.main.transform.position = new Vector3(pos.x, camPos.y, pos.z);
    }

    private void LateUpdate()
    {
        Vector3 targetDir = aimTarget.position - transform.position;
        float angle = Vector2.SignedAngle(new Vector2(transform.forward.x, transform.forward.z), new(targetDir.x, targetDir.z));
        transform.Rotate(Vector3.down, angle);
    }

    private void FixedUpdate()
    {
        Vector2 vel = dir * speed;
        rb.linearVelocity = new Vector3(vel.x, 0f, vel.y);
    }

    private void Death()
    {
        youDied.SetActive(true);
        body.SetActive(true);
        body.transform.parent = null;
        gameObject.SetActive(false);
        AntennaManager.Instance.Died = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.tag == "Crate")
        {
            bool didHeal = false;

            if (other.gameObject.TryGetComponent(out HealthCrate crate))
            {
                int heal = crate.HealAmount;
                didHeal = health.AddHealth(heal);
            }

            if (didHeal)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
