using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ZombieStats : MonoBehaviour
{
    [Header("Attributes - Core Health")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currencyOnDestroy = 30;

    [Header("Attributes - Specific Stats")]
    [SerializeField] private float baseMovementSpeed = 2f;
    [SerializeField] private float damageNegationModifier = 0.0f;

    [Header("Attributes - Zombie Specifics")]
    [SerializeField] private float speedIncreasePerOtherZombie = 0.05f;

    private int currentHealth;
    private bool isDestroyed = false;
    private IMovementHandler movementHandler;

    public delegate void EnemyKilledAction();
    public static event EnemyKilledAction OnEnemyKilled;

    private void Start()
    {
        currentHealth = maxHealth;
        movementHandler = GetComponent<IMovementHandler>();
        if (movementHandler == null)
        {
          
        }

        UpdateMovementSpeed();
    }

    private void OnEnable()
    {
        OnEnemyKilled += UpdateMovementSpeed;
    }

    private void OnDisable()
    {
        OnEnemyKilled -= UpdateMovementSpeed;
    }

    public void UpdateMovementSpeed()
    {
        if (isDestroyed || movementHandler == null) return;

        ZombieStats[] allZombies = FindObjectsByType<ZombieStats>(FindObjectsSortMode.None);

        int otherZombiesCount = allZombies.Length - 1;

        float totalSpeedIncrease = otherZombiesCount * speedIncreasePerOtherZombie;
        float newSpeed = baseMovementSpeed * (1f + totalSpeedIncrease);

        movementHandler.SetSpeed(newSpeed);
    }

    /// <param name="rawDamage">The incoming damage amount.</param>
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

public interface IMovementHandler
{
    void SetSpeed(float newSpeed);
}