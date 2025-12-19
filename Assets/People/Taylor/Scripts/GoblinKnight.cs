using UnityEngine;

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

    private void Start()
    {
        currentHealth = maxHealth;

        if (TryGetComponent(out IMovementHandler movementHandler))
        {
            movementHandler.SetSpeed(baseMovementSpeed);
        }
    }

    public void TakeDamage(int rawDamage)
    {
        if (isDestroyed) return;

        int effectiveDamage = Mathf.RoundToInt(rawDamage * (1f - damageNegationModifier));
        currentHealth -= effectiveDamage;

        if (currentHealth <= 0) Die();
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
