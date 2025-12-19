using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;

    private Transform target;
    private GameObject towerOwner;

    public void SetTarget(Transform targetTransform, GameObject owner)
    {
        target = targetTransform;
        towerOwner = owner;

        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * bulletSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject hitObj = other.gameObject;

        if (hitObj.TryGetComponent(out IceSkeleton iceSkeleton))
        {
            iceSkeleton.TakeDamage(bulletDamage, towerOwner);
        }
        else if (hitObj.TryGetComponent(out RobotHealth robot)) robot.TakeDamage(bulletDamage);
        else if (hitObj.TryGetComponent(out ZombieStats zombie)) zombie.TakeDamage(bulletDamage);
        else if (hitObj.TryGetComponent(out RiotZombie riot)) riot.TakeDamage(bulletDamage);
        else if (hitObj.TryGetComponent(out Slime slime)) slime.TakeDamage(bulletDamage);
        else if (hitObj.TryGetComponent(out Skeleton skeleton)) skeleton.TakeDamage(bulletDamage);
        else if (hitObj.TryGetComponent(out GoblinKnight goblinK)) goblinK.TakeDamage(bulletDamage);
        else if (hitObj.TryGetComponent(out Amalgamation amalgamation)) amalgamation.TakeDamage(bulletDamage);
        else if (hitObj.TryGetComponent(out Goblin goblin)) goblin.TakeDamage(bulletDamage);
        else if (hitObj.TryGetComponent(out MetalSkeleton metalSkeleton)) metalSkeleton.TakeDamage(bulletDamage);
        else if (hitObj.TryGetComponent(out Health generalHealth)) generalHealth.TakeDamage(bulletDamage);

        Destroy(gameObject);
    }
}
