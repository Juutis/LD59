using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public Transform Legs;
    public Transform UpperBody;
    public Animator LegsAnimator;

    private Rigidbody rb;

    private float MAX_FORWARD_ANGLE = 100;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var moveDir = rb.linearVelocity.normalized;

        if (rb.linearVelocity.magnitude < 0.1f)
        {
            moveDir = UpperBody.up;
            LegsAnimator.Play("idle");
        }

        if (Vector3.Angle(moveDir, UpperBody.right) < MAX_FORWARD_ANGLE)
        {
            var angle = Vector3.SignedAngle(Legs.right, moveDir, Legs.forward);
            Legs.Rotate(Vector3.forward, angle);

            if (rb.linearVelocity.magnitude > 0.1f)
            {
                LegsAnimator.Play("run");
            }
        }
        else
        {
            var angle = Vector3.SignedAngle(Legs.right, moveDir, Legs.forward) + 180;
            Legs.Rotate(Vector3.forward, angle);
            LegsAnimator.Play("run_back");
        }
    }
}
