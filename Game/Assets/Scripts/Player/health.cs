using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; private set; }

    public event Action<int> OnHealthChanged; // remaining health
    public event Action OnDied;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth == 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    void Die()
    {
        OnDied?.Invoke();
        // default behaviour: disable object (customize in subclasses)
        gameObject.SetActive(false);
    }
}
