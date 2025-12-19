using System;
using System.Collections;
using UnityEngine;

public class IceSkeleton : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int health = 20;
    [SerializeField] private float movementSpeed = 3.0f;
    [SerializeField] private int currencyValue = 50;

    private bool isDestroyed = false;

    private void Start()
    {
        if (TryGetComponent(out IMovementHandler movement))
            movement.SetSpeed(movementSpeed);
    }

    public void TakeDamage(int damage, GameObject attacker)
    {
        if (isDestroyed) return;
        health -= damage;

        if (health <= 0)
        {
            if (attacker != null) StartCoroutine(StunTurret(attacker));
            Die();
        }
    }

    private IEnumerator StunTurret(GameObject attacker)
    {
        // Find the Turret script on the attacker
        if (attacker.TryGetComponent(out Turret turretScript))
        {
            turretScript.enabled = false; // Freeze the turret logic

            // Optional: Visual feedback (turn blue)
            if (attacker.TryGetComponent(out SpriteRenderer sr)) sr.color = Color.cyan;

            yield return new WaitForSeconds(1.0f); // 1 Second Requirement

            if (turretScript != null)
            {
                turretScript.enabled = true; // Unfreeze
                if (attacker.TryGetComponent(out SpriteRenderer sr2)) sr2.color = Color.white;
            }
        }
    }

    private void Die()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        if (EnemySpawner.onEnemyDestroy != null) EnemySpawner.onEnemyDestroy.Invoke();
        if (LevelManager.main != null) LevelManager.main.AddCurrency(currencyValue);

        // Hide visuals immediately while the stun coroutine finishes
        if (TryGetComponent(out SpriteRenderer sr)) sr.enabled = false;
        if (TryGetComponent(out Collider2D col)) col.enabled = false;

        Destroy(gameObject, 1.1f); // Destroy after stun is finished
    }

    internal void TakeDamage(int bulletDamage)
    {
        throw new NotImplementedException();
    }
}
