using UnityEngine;
using UnityEngine.UIElements;

public class BaseGun : MonoBehaviour
{
    [SerializeField]
    private BulletTrail bulletTrailPrefab;
    [SerializeField]
    GunConfig config;
    [SerializeField]
    private ParticleSystem shootEffect;

    private float rateOfFire;
    private float damage;
    private float range;

    private float lastShot = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rateOfFire = config.RateOfFire;
        damage = config.Damage;
        range = config.Range;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        if (Time.time - lastShot > (1/rateOfFire))
        {
            Debug.Log("Shooting");
            lastShot = Time.time;
            shootEffect.Play();

            var trailEnd = transform.position + transform.forward * range * 4;

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range))
            {
                trailEnd = hit.point;
                Debug.Log("Hit");
            }

            BulletTrail bulletTrail = Instantiate(bulletTrailPrefab);
            bulletTrail.Init(transform.position, trailEnd);
        }
    }
}
