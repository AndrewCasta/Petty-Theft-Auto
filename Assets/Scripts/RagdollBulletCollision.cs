using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollBulletCollision : MonoBehaviour
{
    GameManager gameManager;
    
    Rigidbody mainRb;
    Rigidbody[] ragdollRB;
    Animator animator;
    BloodEffect bloodEffect;

    public int health;

    [SerializeField] float bulletCollisionForce;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ragdollRB = GetComponentsInChildren<Rigidbody>();
        mainRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        bloodEffect = GameObject.Find("SpawnManager").GetComponent<BloodEffect>();
        DisableRagdoll();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("bullet") && !collision.gameObject.GetComponent<Bullet>().hasHit)
        {
            // Set the bullet to 'hasHit' so it won't hit again.
            collision.gameObject.GetComponent<Bullet>().hasHit = true;

            // Remove health and check if dead
            health--;
            if (health > 0) return;
            if (health < 1 && gameObject.name == "Player") gameManager.GameOver();

            if (gameObject.name == "Player") return;

            EnableRagdoll();
            // Find closest ragdoll collider
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
            // Handle hitbox hit
            // Debug.Log($"Closet Hitbox: {closestHitbox.gameObject.name}");
            Vector3 direction = (closestHitbox.gameObject.transform.position - collision.gameObject.transform.position).normalized;

            // Add blood effect
            bloodEffect.SpawnBloodEffect(closestHitbox.gameObject);
            // Apply force
            closestHitbox.gameObject.GetComponent<Rigidbody>().AddForce(-direction * bulletCollisionForce, ForceMode.Impulse);
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
        if(animator != null) animator.enabled = true;
    }

    void EnableRagdoll()
    {
        if (animator != null) animator.enabled = false;
        mainRb.constraints = RigidbodyConstraints.None;
        foreach (Rigidbody rb in ragdollRB)
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
        }
        mainRb.isKinematic = true;
        mainRb.detectCollisions = false;
    }
}
