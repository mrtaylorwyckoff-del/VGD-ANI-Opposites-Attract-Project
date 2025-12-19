using UnityEngine;

public class RobotHealth : MonoBehaviour
{
    [Header("Attributes - Core Health")]
    [SerializeField] private int maxHealth = 7;
    [SerializeField] private int currencyOnDestroy = 35;

    [Header("Attributes - Specific Stats")]
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private float damageNegationModifier = 0.20f;

    private int currentHealth;
    private bool isDestroyed = false;

    public delegate void RobotKilledAction();
    public static event RobotKilledAction OnAnyRobotKilled;

    private void Start() => currentHealth = maxHealth;

    private void OnEnable() => OnAnyRobotKilled += RegainHealth;
    private void OnDisable() => OnAnyRobotKilled -= RegainHealth;

    public void TakeDamage(int rawDamage)
    {
        if (isDestroyed) return;

        int effectiveDamage = Mathf.RoundToInt(rawDamage * (1f - damageNegationModifier));
        currentHealth -= effectiveDamage;

        if (currentHealth <= 0) Die();
    }

    private void RegainHealth()
    {
        if (isDestroyed) return;

        currentHealth = Mathf.Min(currentHealth + 1, maxHealth);
    }

    private void Die()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        OnAnyRobotKilled?.Invoke();

        if (EnemySpawner.onEnemyDestroy != null) EnemySpawner.onEnemyDestroy.Invoke();
        if (LevelManager.main != null) LevelManager.main.AddCurrency(currencyOnDestroy);

        Destroy(gameObject);
    }
}
