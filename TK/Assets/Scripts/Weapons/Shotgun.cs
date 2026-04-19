using UnityEngine;

public class Shotgun : BaseGun
{
    [SerializeField]
    private int shots = 5;

    // Update is called once per frame
    void Update()
    {
        
    }

    public new void Shoot()
    {
        Debug.Log($"Shotgun pum {Time.time - lastShot} | {(1 / rateOfFire)} | {(Time.time - lastShot) > (1 / rateOfFire)}");
        if (Time.time - lastShot > (1 / rateOfFire))
        {
            Debug.Log("Shooting");
            lastShot = Time.time;
            shootEffect.Play();

            float angle = -(spreadAngle / 2f);
            float angleStep = spreadAngle / 5f;

            for (int i = 0; i < shots; i++)
            {
                float currentAngle = angle + angleStep * i;
                Vector3 dir = Quaternion.AngleAxis(currentAngle, Vector3.up) * transform.forward;

                var trailEnd = transform.position + dir * range * 4;

                if (Physics.Raycast(transform.position, dir, out RaycastHit hit, range))
                {
                    trailEnd = hit.point;
                    Debug.Log("Hit");

                    if (hit.collider.tag == "Enemy")
                    {
                        if (hit.collider.gameObject.TryGetComponent(out CharacterHealth enemy))
                        {
                            enemy.Hurt(damage);
                        }
                    }
                }

                BulletTrail bulletTrail = Instantiate(bulletTrailPrefab);
                bulletTrail.Init(transform.position, trailEnd);
            }
        }

    }
}
