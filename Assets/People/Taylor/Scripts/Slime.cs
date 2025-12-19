using UnityEngine;

public class Slime : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int health = 20;
    [SerializeField] private float movementSpeed = 1.0f; 
    [SerializeField] private int currencyValue = 60;

    [Header("Resistance")]
    [Tooltip("0.5 means the slime takes 50% of incoming damage.")]
    [SerializeField] private float damageMultiplier = 0.5f;

    private bool isDead = false;

    public void TakeDamage(int rawDamage)
    {
        if (isDead) return;

        int effectiveDamage = Mathf.CeilToInt(rawDamage * damageMultiplier);
        health -= effectiveDamage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (LevelManager.main != null)
            LevelManager.main.AddCurrency(currencyValue);

        if (EnemySpawner.onEnemyDestroy != null)
            EnemySpawner.onEnemyDestroy.Invoke();

        Destroy(gameObject);
    }

    public float GetSpeed() => movementSpeed;
}
