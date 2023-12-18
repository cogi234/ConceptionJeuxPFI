using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageableComponent : MonoBehaviour
{
    //Events for taking damage and healing
    /// <summary>
    /// Damage taken as argument
    /// </summary>
    public UnityEvent<int> onDamage = new UnityEvent<int>();
    /// <summary>
    /// Damage healed as argument
    /// </summary>
    public UnityEvent<int> onHeal = new UnityEvent<int>();

    //For invincibility timer
    public float invincibilityTime = 2;
    float invincibilityTimer;

    public void TakeDamage(int damage)
    {
        if (!enabled)
            return;

        //If we're not invincible
        if (invincibilityTimer <= 0)
        {
            //We take damage
            onDamage.Invoke(damage);

            //We set the invincibility timer
            invincibilityTimer = invincibilityTime;
        }
    }

    public void Heal(int healing)
    {
        if (!enabled)
            return;
        onHeal.Invoke(healing);
    }

    private void Update()
    {
        if (!enabled)
            return;

        //Invincibility timer
        if (invincibilityTimer > 0)
            invincibilityTimer -= Time.deltaTime;
    }
}
