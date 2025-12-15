using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPathfinding : MonoBehaviour
{
    private Animator animator;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb; 
    
    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private Transform target;
    private int pathIndex = 0;

    private void Start()
    {
        target = LevelManager.main.path[pathIndex];

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            if (pathIndex >= LevelManager.main.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }

        Vector2 direction = (target.position - transform.position).normalized;
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);

        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = direction.x < 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }
}