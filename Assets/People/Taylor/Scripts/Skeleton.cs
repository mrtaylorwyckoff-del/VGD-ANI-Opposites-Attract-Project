using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [Header("Unit Statistics")]
    public string unitName = "Skeleton";
    public int health = 2;
    public float movementSpeed = 3f;
    public int armor = 0;
    public int cost = 15;

    [Header("State")]
    private bool isDead = false;

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        Debug.Log($"{unitName} took {damage} damage. Remaining HP: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log($"{unitName} has crumbled!");

        Destroy(gameObject);
    }
}
