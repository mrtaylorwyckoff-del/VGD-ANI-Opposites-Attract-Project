using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 3;

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            //EnemySpawner.onEnemyDestroy.Invoke();
            Destroy(gameObject);
        }
    }
}
