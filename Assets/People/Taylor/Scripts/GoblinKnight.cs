using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GoblinKnight : MonoBehaviour
{
    [Header("Attributes - Core Health")]
    [SerializeField] private int maxHealth = 12;
    [SerializeField] private int currencyOnDestroy = 50;

    [Header("Attributes - Specific Stats")]
    [SerializeField] private float baseMovementSpeed = 2f;
    [SerializeField] private float damageNegationModifier = 0.0f;

    private int currentHealth;
    private bool isDestroyed = false;
    private IMovementHandler movementHandler;

    public delegate void EnemyKilledAction();
    public static event EnemyKilledAction OnEnemyKilled;

    private void Start()
    {
        currentHealth = maxHealth;
        movementHandler = GetComponent<IMovementHandler>();

        if (movementHandler != null)
        {
            movementHandler.SetSpeed(baseMovementSpeed);
        }
    }

    public void TakeDamage(int rawDamage)
    {
        if (isDestroyed) return;

        int effectiveDamage = Mathf.RoundToInt(rawDamage * (1f - damageNegationModifier));
        currentHealth -= effectiveDamage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        OnEnemyKilled?.Invoke();

        if (EnemySpawner.onEnemyDestroy != null) EnemySpawner.onEnemyDestroy.Invoke();
        if (LevelManager.main != null) LevelManager.main.AddCurrency(currencyOnDestroy);

        Destroy(gameObject);
    }

    public int CurrentHealth => currentHealth;
    public float BaseMovementSpeed => baseMovementSpeed;
    public float DamageNegationModifier => damageNegationModifier;
    public int Cost => currencyOnDestroy;
}
