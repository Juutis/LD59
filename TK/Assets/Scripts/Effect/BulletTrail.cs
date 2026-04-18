using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    private TrailRenderer trail;
    private float lifeTime = 0.2f;

    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        Invoke("DestroyThis", lifeTime);
    }

    public void Init(Vector3 start, Vector3 end)
    {
        trail = GetComponent<TrailRenderer>();
        trail.AddPosition(start);
        trail.AddPosition(end);
        transform.position = end;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
