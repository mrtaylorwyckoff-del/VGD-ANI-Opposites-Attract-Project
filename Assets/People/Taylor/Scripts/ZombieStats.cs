using UnityEngine;

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
        UpdateMovementSpeed();
    }

    private void OnEnable() => OnEnemyKilled += UpdateMovementSpeed;
    private void OnDisable() => OnEnemyKilled -= UpdateMovementSpeed;

    public void UpdateMovementSpeed()
    {
        if (isDestroyed || movementHandler == null) return;

        ZombieStats[] allZombies = Object.FindObjectsByType<ZombieStats>(FindObjectsSortMode.None);
        int otherZombiesCount = allZombies.Length - 1;

        float totalSpeedIncrease = otherZombiesCount * speedIncreasePerOtherZombie;
        float newSpeed = baseMovementSpeed * (1f + totalSpeedIncrease);

        movementHandler.SetSpeed(newSpeed);
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
        isDestroyed = true;
        OnEnemyKilled?.Invoke();

        if (EnemySpawner.onEnemyDestroy != null) EnemySpawner.onEnemyDestroy.Invoke();
        if (LevelManager.main != null) LevelManager.main.AddCurrency(currencyOnDestroy);

        Destroy(gameObject);
    }
}

public interface IMovementHandler
{
    void SetSpeed(float newSpeed);
}
