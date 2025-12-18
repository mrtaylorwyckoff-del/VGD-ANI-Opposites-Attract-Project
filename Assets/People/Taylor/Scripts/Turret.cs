using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private SpriteRenderer baseSpriteRenderer; // optional, used for flipping the base sprite only

    [Header("Attributes")]
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float bps = 1f; // bullets per second
    [SerializeField] private float targetingRange = 5f;

    private Transform target;
    private float timeUntilFire;
    private bool isFacingLeft;

    private void Awake()
    {
        // Try to auto-assign a SpriteRenderer if none set in inspector
        if (baseSpriteRenderer == null)
            baseSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (target == null)
            FindTarget();

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

        // Base flip: don't change transform.rotation (that's flipping child axes).
        // Use SpriteRenderer.flipX (or localScale sign) so child rotation and firing direction stay consistent.
        if (target != null)
        {
            // Use turretRotationPoint (or turret root) position to determine left/right
            Vector3 origin = (turretRotationPoint != null) ? turretRotationPoint.position : transform.position;
            Vector3 direction = target.position - origin;
            bool shouldFaceLeft = direction.x < 0f;

            if (baseSpriteRenderer != null)
            {
                // flip sprite only (visual). This does not change child rotations.
                baseSpriteRenderer.flipX = shouldFaceLeft;
            }
            else
            {
                // fallback: flip localScale.x � but be cautious: scaling affects child rotations/positions.
                Vector3 s = transform.localScale;
                if (shouldFaceLeft != isFacingLeft)
                {
                    s.x = Mathf.Abs(s.x) * (shouldFaceLeft ? -1f : 1f);
                    transform.localScale = s;
                }
            }

            isFacingLeft = shouldFaceLeft;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firingPoint == null) return;

        // Instantiate using firingPoint rotation so bullet faces the barrel direction
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);

        // If bullet expects a target, keep SetTarget behavior.
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(target);
        }
        else
        {
            // If Bullet script is not present, try to set velocity directly (2D example).
            Rigidbody2D rb2d = bulletObj.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                // Assume the bullet sprite faces right (+X). Use firingPoint.right as forward.
                rb2d.linearVelocity = firingPoint.right * (rb2d.linearVelocity.magnitude > 0 ? rb2d.linearVelocity.magnitude : 5f);
            }
        }
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

        // Use the turretRotationPoint position to compute the angle so rotation pivots correctly
        Vector3 origin = turretRotationPoint.position;
        float angle = Mathf.Atan2(target.position.y - origin.y, target.position.x - origin.x) * Mathf.Rad2Deg;

        // If you flipped the base with localScale (not flipX) the local axis might be mirrored.
        // Using SpriteRenderer.flipX avoids needing to invert the angle. If you use localScale flipping,
        // uncomment the following line to invert angle when facing left:
        // if (transform.localScale.x < 0f) angle = 180f - angle;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        turretRotationPoint.rotation = Quaternion.Lerp(turretRotationPoint.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }
}