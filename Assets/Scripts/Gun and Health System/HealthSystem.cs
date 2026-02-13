using System;
using Unity.Netcode;
using UnityEngine;

public class HealthSystem : NetworkBehaviour
{
   [SerializeField] private float maxHealth = 100f;
    public static Action<float> OnTakeDamage;
    public static Action OnDeath;
     public void TakeDamage(float amount)
    {
        maxHealth -= amount;
        OnTakeDamage?.Invoke(maxHealth);
        if (maxHealth <= 0)
            Die();
    }

    void Die()
    {
        OnDeath?.Invoke();
        Debug.Log("DEAD");
        //Disable the player then enable after a time interval at a random spawn Position with full health that's it
    }
}
