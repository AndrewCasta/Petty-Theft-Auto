using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollBulletCollision : MonoBehaviour
{
    Rigidbody mainRb;
    Rigidbody[] ragdollRB;
    Animator animator;

    [SerializeField] float bulletCollisionForce;

    private void Awake()
    {
        ragdollRB = GetComponentsInChildren<Rigidbody>();
        mainRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        DisableRagdoll();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            // Find closest ragdoll collider
            EnableRagdoll();
            Collider[] hitColliders = Physics.OverlapSphere(collision.gameObject.transform.position, 2f);
            Collider bulletCollider = collision.gameObject.GetComponent<Collider>();
            Collider closestHitbox = null;
            float closestHitboxDistance = 10f;
            foreach (Collider hitbox in hitColliders)
            {
                if (hitbox != bulletCollider)
                {
                    float hitboxDistance = (bulletCollider.gameObject.transform.position - hitbox.gameObject.transform.position).sqrMagnitude;
                    if (hitboxDistance < closestHitboxDistance)
                    {
                        closestHitboxDistance = hitboxDistance;
                        closestHitbox = hitbox;
                    }
                    // Debug.Log($"Hitbox: {hitbox.gameObject.name} - Distance: {hitboxDistance}");
                }
            }
            // Apply force
            if (closestHitbox)
            {
                Debug.Log($"Closet Hitbox: {closestHitbox.gameObject.name}");
                Vector3 direction = (closestHitbox.gameObject.transform.position - collision.gameObject.transform.position).normalized;
                closestHitbox.gameObject.GetComponent<Rigidbody>().AddForce(-direction * bulletCollisionForce, ForceMode.Impulse);
            }

        }
    }

    void DisableRagdoll()
    {
        foreach (Rigidbody rb in ragdollRB)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
        mainRb.isKinematic = false;
        mainRb.detectCollisions = true;
        animator.enabled = true;
    }

    void EnableRagdoll()
    {
        animator.enabled = false;
        foreach (Rigidbody rb in ragdollRB)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }
        mainRb.isKinematic = true;
        mainRb.detectCollisions = false;
    }
}
