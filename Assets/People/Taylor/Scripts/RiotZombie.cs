using System.Collections;
using UnityEngine;

public class RiotZombie : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 60;
    public float baseMovementSpeed = 1.0f;
    public float damageNegationPercent = 0.75f;
    public int costValue = 100;

    private int currentHealth;
    private bool hasArmor = true;
    private bool isStunned = false;
    private const float stunDuration = 5.0f;
    private int halfHealthThreshold;

    private void Start()
    {
        currentHealth = maxHealth;
        halfHealthThreshold = maxHealth / 2;

        if (TryGetComponent(out IMovementHandler movement)) movement.SetSpeed(baseMovementSpeed);
    }

    public void TakeDamage(int amount)
    {
        if (isStunned) return;

        float multiplier = hasArmor ? (1f - damageNegationPercent) : 1f;
        currentHealth -= Mathf.RoundToInt(amount * multiplier);

        if (hasArmor && currentHealth <= halfHealthThreshold)
        {
            StartCoroutine(RemoveArmorAndStun());
        }

        if (currentHealth <= 0) Die();
    }

    private IEnumerator RemoveArmorAndStun()
    {
        hasArmor = false;
        isStunned = true;

        if (TryGetComponent(out IMovementHandler movement)) movement.SetSpeed(0f);

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;

        if (movement != null) movement.SetSpeed(baseMovementSpeed);
    }

    private void Die()
    {
        if (LevelManager.main != null) LevelManager.main.AddCurrency(costValue);
        if (EnemySpawner.onEnemyDestroy != null) EnemySpawner.onEnemyDestroy.Invoke();

        Destroy(gameObject);
    }
}
