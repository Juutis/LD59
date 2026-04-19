using UnityEngine;

public class Shotgun : BaseGun
{
    [SerializeField]
    private int shots = 5;

    // Update is called once per frame
    void Update()
    {
        
    }

    public new bool Shoot()
    {
        if (Time.time - lastShot > (1 / rateOfFire))
        {
            lastShot = Time.time;
            shootEffect.Play();

            float angle = -(spreadAngle / 2f);
            float angleStep = spreadAngle / 4f;

            for (int i = 0; i < shots; i++)
            {
                float randomSpread = Random.Range(0, spreadAngle / 10f);
                float currentAngle = randomSpread + angle + angleStep * i;
                Vector3 dir = Quaternion.AngleAxis(currentAngle, Vector3.up) * transform.forward;

                var trailEnd = transform.position + dir * range * 4;

                if (Physics.Raycast(transform.position, dir, out RaycastHit hit, range))
                {
                    trailEnd = hit.point;

                    if (hit.collider.tag == "Enemy")
                    {
                        if (hit.collider.gameObject.TryGetComponent(out CharacterHealth enemy))
                        {
                            enemy.Hurt(damage, dir);
                        }
                    }
                }

                BulletTrail bulletTrail = Instantiate(bulletTrailPrefab);
                bulletTrail.Init(transform.position, trailEnd);
            }

            return true;
        }

        return false;
    }
}
