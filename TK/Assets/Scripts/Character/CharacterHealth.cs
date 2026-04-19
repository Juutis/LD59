using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    private float health;
    private float maxHealth;
    private Action deathAction;

    [SerializeField]
    private GameObject bloodPrefab;

    private bool dead = false;

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

        if (health <= 0 && !dead)
        {
            transform.LookAt(transform.position - direction, Vector3.up);
            dead = true;
            Kill();
        }
    }

    public void Kill()
    {
        deathAction();
    }
}
