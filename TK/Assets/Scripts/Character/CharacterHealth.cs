using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    private float health;
    private float maxHealth;
    private Action deathAction;

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

    public void Hurt(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        deathAction();
    }
}
