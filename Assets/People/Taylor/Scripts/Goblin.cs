using UnityEngine;

public class Goblin : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int health = 3;
    [SerializeField] private int killReward = 25;
    [SerializeField] private float speed = 2f;

    private bool isDead = false;

    private void Start()
    {
        var movement = GetComponent<IMovementHandler>();
        if (movement != null) movement.SetSpeed(speed);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        if (health <= 0) Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (LevelManager.main != null)
            LevelManager.main.AddCurrency(killReward);

        EnemySpawner.onEnemyDestroy?.Invoke();

        Destroy(gameObject);
    }
}
