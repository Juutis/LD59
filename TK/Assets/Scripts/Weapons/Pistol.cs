using UnityEngine;

public class Pistol : BaseGun
{
    // Update is called once per frame
    void Update()
    {
    }

    public new bool Shoot()
    {
        if ((Time.time - lastShot) > (1 / rateOfFire))
        {
            lastShot = Time.time;
            shootEffect.Play();

            float currentAngle = Random.Range(0, spreadAngle) - spreadAngle / 2f;
            Vector3 dir = Quaternion.AngleAxis(currentAngle, Vector3.up) * transform.forward;

            var trailEnd = transform.position + dir * range;

            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, range, targetLayerMask))
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

            return true;
        }

        return false;
    }
}
