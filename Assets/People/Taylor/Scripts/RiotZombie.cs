using System.Collections;
using UnityEngine;

public class RiotZombie : MonoBehaviour
{
    public int maxHealth = 60;
    public float baseMovementSpeed = 1.0f;
    public float stunnedMovementSpeed = 0.0f;
    public float damageNegationPercent = 0.75f;
    public int costValue = 100;

    private int currentHealth;
    private bool hasArmor = true;
    private bool isStunned = false;
    private const float stunDuration = 5.0f;
    private int halfHealthThreshold;

    private void Start()
    {
        currentHealth = maxHealth;
        halfHealthThreshold = maxHealth / 2;

    }

    /// <param name="amount">The amount of raw damage before armor calculation.</param>
    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0 || isStunned) return;

        float effectiveDamage = amount;

        if (hasArmor)
        {
            effectiveDamage *= (1 - damageNegationPercent);
        }

        currentHealth -= Mathf.RoundToInt(effectiveDamage);

        Debug.Log($"Took {effectiveDamage} effective damage. Current Health: {currentHealth}");

        if (hasArmor && currentHealth <= halfHealthThreshold)
        {
            RemoveArmorAndStun();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void RemoveArmorAndStun()
    {
        hasArmor = false;
        Debug.Log("Armor destroyed! Zombie is stunned.");

        if (!isStunned)
        {
            StartCoroutine(StunRoutine());
        }
    }

    private IEnumerator StunRoutine()
    {
        isStunned = true;

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;

        Debug.Log("Zombie is no longer stunned.");
    }

    private void Die()
    {
        Debug.Log("Riot Zombie destroyed. Player gains $" + costValue);
        Destroy(gameObject);
    }
}
