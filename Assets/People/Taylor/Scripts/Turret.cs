using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Attributes")]
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float bps = 1f;
    [SerializeField] private float targetingRange = 5f;

    private Transform target;
    private float timeUntilFire;

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
            return;
        }

        timeUntilFire -= Time.deltaTime;
        if (timeUntilFire <= 0f)
        {
            Shoot();
            timeUntilFire = 1f / Mathf.Max(0.0001f, bps);
        }

        if (target != null)
        {
            Vector3 direction = target.position - transform.position;

            if (direction.x < 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private void Shoot()
    {   
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target, gameObject);
    }

    private void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);
        if (hits.Length > 0)
        {
            float bestDist = float.MaxValue;
            Transform best = null;
            foreach (var c in hits)
            {
                float d = Vector2.Distance(transform.position, c.transform.position);
                if (d < bestDist)
                {
                    bestDist = d;
                    best = c.transform;
                }
            }
            target = best;
        }
    }

    private bool CheckTargetIsInRange()
    {
        if (target == null) return false;
        return Vector2.Distance(transform.position, target.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        if (target == null || turretRotationPoint == null) return;

        turretRotationPoint.right =-(target.position-turretRotationPoint.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }
}