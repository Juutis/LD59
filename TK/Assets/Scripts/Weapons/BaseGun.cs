using UnityEngine;
using UnityEngine.UIElements;

public class BaseGun : MonoBehaviour
{
    [SerializeField]
    protected BulletTrail bulletTrailPrefab;
    [SerializeField]
    protected GunConfig config;
    [SerializeField]
    protected ParticleSystem shootEffect;

    protected float rateOfFire;
    protected float damage;
    protected float range;
    protected float spreadAngle;

    protected float lastShot = 0;

    private void Start()
    {
        Init();
    }

    protected void Init()
    {
        rateOfFire = config.RateOfFire;
        damage = config.Damage;
        range = config.Range;
        spreadAngle = config.SpreadAngle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
    }
}
