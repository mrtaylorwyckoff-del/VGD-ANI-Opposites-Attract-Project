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

    public delegate void SlimeKilledAction();
    public static event SlimeKilledAction OnSlimeKilled;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int rawDamage)
    {
        if (isDestroyed) return;

        float effectiveDamageFloat = rawDamage * (1f - damageNegationModifier);
        int effectiveDamage = Mathf.CeilToInt(effectiveDamageFloat);

        currentHealth -= effectiveDamage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

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
        OnSlimeKilled?.Invoke();

        if (LevelManager.main != null) LevelManager.main.AddCurrency(currencyOnDestroy);
        if (EnemySpawner.onEnemyDestroy != null) EnemySpawner.onEnemyDestroy.Invoke();

        Destroy(gameObject);
    }

    private void OnEnable()
    {
        OnSlimeKilled += RegainHealthOnOtherDeath;
    }

    private void OnDisable()
    {
        OnSlimeKilled -= RegainHealthOnOtherDeath;
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
