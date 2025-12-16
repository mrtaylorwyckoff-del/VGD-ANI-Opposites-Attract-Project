using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attributes")]
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float bps = 1f; // bullets per second
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

        // count down and fire when <= 0
        timeUntilFire -= Time.deltaTime;
        if (timeUntilFire <= 0f)
        {
            Shoot();
            timeUntilFire = 1f / Mathf.Max(0.0001f, bps);
        }

        //adds animation to turn turret left or right based on target position
        if (target != null && turretRotationPoint != null)
        {
            Vector3 direction = target.position - transform.position;
            if (direction.x < 0)
            {
                // Target is to the left
                turretRotationPoint.localScale = new Vector3(-1, 1, 1); // Flip the turret
            }
            else
            {
                // Target is to the right
                turretRotationPoint.localScale = new Vector3(1, 1, 1); // Normal orientation
            }
        }
}

    private void Shoot()
    {   
       GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    private void FindTarget()
    {
        // Use OverlapCircleAll for an immediate range check
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);
        if (hits.Length > 0)
        {
            // pick the closest
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

        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        turretRotationPoint.rotation = Quaternion.Lerp(turretRotationPoint.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }
}