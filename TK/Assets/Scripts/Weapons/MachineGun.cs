using UnityEngine;

public class MachineGun : BaseGun
{
    // Update is called once per frame
    void Update()
    {
        
    }

    public new bool Shoot()
    {
        Debug.Log($"Big gun pum pum {Time.time - lastShot} | {(1 / rateOfFire)} | {(Time.time - lastShot) > (1 / rateOfFire)}");
        if ((Time.time - lastShot) > (1 / rateOfFire))
        {
            Debug.Log("Shooting");
            lastShot = Time.time;
            shootEffect.Play();

            float currentAngle = Random.Range(0, spreadAngle) - spreadAngle / 2f;
            Vector3 dir = Quaternion.AngleAxis(currentAngle, Vector3.up) * transform.forward;

            var trailEnd = transform.position + dir * range * 4;

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range))
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

            return true;
        }

        return false;
    }
}
