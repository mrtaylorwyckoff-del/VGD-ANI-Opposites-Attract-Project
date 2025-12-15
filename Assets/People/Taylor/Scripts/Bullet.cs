using Unity.Cinemachine;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Bullet : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private int bulletDamage = 1;

    private Transform target;

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;

        rb.linearVelocity = direction * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        RobotHealth robotHealth = other.gameObject.GetComponent<RobotHealth>();
        if (robotHealth != null)
        {
            robotHealth.TakeDamage(bulletDamage);
        }

        ZombieStats zombieStats = other.gameObject.GetComponent<ZombieStats>();
        if (zombieStats != null)
        {
            zombieStats.TakeDamage(bulletDamage);
        }

        Health health = other.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(bulletDamage);
        }

        RiotZombie riotZombie = other.gameObject.GetComponent<RiotZombie>();
        if (riotZombie != null)
        {
            riotZombie.TakeDamage(bulletDamage);
        }

        Slime slime = other.gameObject.GetComponent<Slime>();
        if (slime != null)
        {
            slime.TakeDamage(bulletDamage);
        }
    }
}
