using UnityEngine;

public class Amalgamation : MonoBehaviour
{
    [Header("Attributes - Core Health")]
    [SerializeField] private int maxHealth = 300;
    [SerializeField] private int currencyOnDestroy = 500;

    [Header("Attributes - Specific Stats")]
    [SerializeField] private float movementSpeed = 1.0f;
    [Tooltip("0.5 means it only takes half of the incoming damage.")]
    [SerializeField] private float damageMultiplier = 0.5f;

    private int currentHealth;
    private bool isDestroyed = false;

    private void Start()
    {
        currentHealth = maxHealth;

        if (TryGetComponent(out IMovementHandler movementHandler))
        {
            movementHandler.SetSpeed(movementSpeed);
        }
    }

    public void TakeDamage(int rawDamage)
    {
        if (isDestroyed) return;

        int effectiveDamage = Mathf.CeilToInt(rawDamage * damageMultiplier);
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

        if (EnemySpawner.onEnemyDestroy != null) EnemySpawner.onEnemyDestroy.Invoke();
        if (LevelManager.main != null) LevelManager.main.AddCurrency(currencyOnDestroy);

        Destroy(gameObject);
    }
}
