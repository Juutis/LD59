using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    private float health;
    private float maxHealth;
    private Action deathAction;

    [SerializeField]
    private GameObject bloodPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitHealth(float initialHealth, Action deathAction)
    {
        health = initialHealth;
        maxHealth = initialHealth;
        this.deathAction = deathAction;
    }

    public void Hurt(float damage, Vector3 direction)
    {
        health -= damage;
        var blood = Instantiate(bloodPrefab);
        blood.transform.position = transform.position;
        direction.y = 0;
        blood.transform.LookAt(transform.position - direction, Vector3.up);

        if (health <= 0)
        {
            transform.LookAt(transform.position - direction, Vector3.up);
            Kill();
        }
    }

    public void Kill()
    {
        deathAction();
    }
}
