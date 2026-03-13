using UnityEngine;

public class ZombieRagdoll : MonoBehaviour, IHittable
{
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;
    private Animator animator;
    private ZombieWander wander;
    private bool isDead = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        wander = GetComponent<ZombieWander>();

        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        // Start in animated state
        SetRagdollActive(false);
    }

    private void OnEnable()
    {
        isDead = false;
        SetRagdollActive(false);
    }

    public void OnHit(Vector3 hitDirection, float hitForce)
    {
        if (isDead) return;
        isDead = true;

        wander.OnDeath();           // Stop wandering
        SetRagdollActive(true);     // Enable ragdoll physics
        ApplyForce(hitDirection, hitForce);  // Push it

        
        
        GameManager.Instance?.AddScore(1); // score goes here — protected by isDead guard
        ZombiePool.Instance.OnZombieDied(); // Tell the pool a zombie just died
    }
 

    private void SetRagdollActive(bool active)
    {
        animator.enabled = !active;

        foreach (Rigidbody rb in ragdollBodies)
        {
            // Skip the root rigidbody - that's the capsule, not a ragdoll bone
            if (rb.transform == transform) continue;
            rb.isKinematic = !active;
        }

        foreach (Collider col in ragdollColliders)
        {
            if (col.transform == transform) continue;
            col.enabled = active;
        }
    }

    private void ApplyForce(Vector3 direction, float force)
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            if (rb.transform == transform) continue;
            rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        }
    }
}