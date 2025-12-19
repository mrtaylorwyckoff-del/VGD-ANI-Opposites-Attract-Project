using UnityEngine;

public class MetalSkeleton : MonoBehaviour
{
    [Header("Attributes - Core Health")]
    [SerializeField] private int maxHealth = 8;
    [SerializeField] private int currencyOnDestroy = 30;

    [Header("Attributes - Specific Stats")]
    [SerializeField] private float movementSpeed = 3.0f;
    [Tooltip("0.20 means it negates 20% of incoming damage.")]
    [SerializeField] private float damageNegationModifier = 0.20f;

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

        float effectiveDamageFloat = rawDamage * (1f - damageNegationModifier);
        int effectiveDamage = Mathf.RoundToInt(effectiveDamageFloat);

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
