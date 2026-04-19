using UnityEngine;

public class Pistol : BaseGun
{
    // Update is called once per frame
    void Update()
    {
        
    }

    public new void Shoot()
    {
        Debug.Log($"Pistol pum {Time.time - lastShot} | {(1 / rateOfFire)} | {(Time.time - lastShot) > (1 / rateOfFire)}");
        if ((Time.time - lastShot) > (1 / rateOfFire))
        {
            Debug.Log("Shooting");
            lastShot = Time.time;
            shootEffect.Play();

            var trailEnd = transform.position + transform.forward * range * 4;

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
        }
    }
}
