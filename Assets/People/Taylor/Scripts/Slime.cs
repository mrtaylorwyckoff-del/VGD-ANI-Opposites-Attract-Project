using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Slime : MonoBehaviour
{
    [Header("Attributes - Core Health")]
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private int currencyOnDestroy = 60;

    [Header("Attributes - Specific Stats")]
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private float damageNegationModifier = 0.50f;

    private int currentHealth;
    private bool isDestroyed = false;

    public delegate void RobotKilledAction();
    public static event RobotKilledAction OnRobotKilled;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    /// <param name="rawDamage">The incoming damage amount.</param>
    public void TakeDamage(int rawDamage)
    {
        if (isDestroyed) return;

        float effectiveDamageFloat = rawDamage * (1f - damageNegationModifier);
        int effectiveDamage = Mathf.RoundToInt(effectiveDamageFloat);

        currentHealth -= effectiveDamage;

        Debug.Log($"Slime took {effectiveDamage} effective damage (halved from {rawDamage}). Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <param name="healAmount">The amount of health to restore.</param>
    public void Heal(int healAmount)
    {
        if (isDestroyed) return;

        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void Die()
    {
        isDestroyed = true;

        OnRobotKilled?.Invoke();

        Destroy(gameObject);
    }

    private void OnEnable()
    {
        OnRobotKilled += RegainHealthOnOtherDeath;
    }

    private void OnDisable()
    {
        OnRobotKilled -= RegainHealthOnOtherDeath;
    }

    private void RegainHealthOnOtherDeath()
    {
        if (!isDestroyed)
        {
            Heal(1);
        }
    }

    public int CurrentHealth => currentHealth;
    public float MovementSpeed => movementSpeed;
    public float DamageNegationModifier => damageNegationModifier;
    public int Cost => currencyOnDestroy;
}