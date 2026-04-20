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

    [SerializeField]
    protected AudioSource audioSource;

    [SerializeField]
    protected AudioClip gunSound;

    protected float rateOfFire;
    protected float damage;
    protected float range;
    protected float spreadAngle;

    protected float lastShot = 0;
    protected int targetLayerMask;

    private void Start()
    {
        Init();
        targetLayerMask = LayerMask.GetMask("World", "Enemy");
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

    public bool Shoot()
    {
        return false;
    }
}
