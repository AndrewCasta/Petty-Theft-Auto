using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopMovement : MonoBehaviour
{
    Rigidbody copRb;
    Rigidbody[] ragdollRB;
    Collider[] ragdollColliders;
    [SerializeField] bool isAlive;
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;
    Animator animator;

    [SerializeField] float distanceToShoot;
    [SerializeField] bool readyToShoot;
    [SerializeField] float reloadTime;
    float reloadTimer;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletForce;


    [SerializeField] float bulletCollisionForce;

    GameObject copBulletSpawnObj;

    float distanceToPlayer;
    GameObject player;
    Vector3 directionToPlayer;

    SpawnManager spawnManager;
    GameManager gameManager;
    float[] gameBounds;

    AudioSource playerAudio;
    [SerializeField] AudioClip gunshotSound;

    private void Awake()
    {
        ragdollRB = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        copRb = GetComponent<Rigidbody>();
        DisableRagdoll();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        isAlive = true;
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameBounds = gameManager.GetBoundry();
        copBulletSpawnObj = transform.Find("Bullet Point").gameObject;
        playerAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isAlive && !InBoundry())
        {
            Destroy(gameObject);
        }
        reloadTimer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (isAlive) AliveBehaviour();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet") && isAlive)
        {
            isAlive = false;
            gameManager.copCount--;
            EnableRagdoll();
            Vector3 direction = (collision.GetContact(0).point - transform.position).normalized;
            copRb.AddForce(-direction * bulletCollisionForce, ForceMode.Impulse);
        }
    }

    void AliveBehaviour()
    {
        // Turn towards player
        directionToPlayer = (player.transform.position - transform.position).normalized;
        // Smooth turning
        Quaternion toRotation = Quaternion.LookRotation(directionToPlayer);
        toRotation.x = 0;
        toRotation.z = 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.deltaTime);

        // Chasing player & shooting range check
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        if (distanceToPlayer > distanceToShoot)
        {
            readyToShoot = false;
            animator.SetBool("isMoving", true);
            transform.position = transform.position + (transform.forward * moveSpeed * Time.deltaTime);
        }
        else
        {
            readyToShoot = true;
            animator.SetBool("isMoving", false);
        }
        if (readyToShoot && reloadTimer > reloadTime) FireBullet();
    }

    bool InBoundry()
    {
        if (transform.position.z > gameBounds[0] || transform.position.x > gameBounds[1] || transform.position.z < gameBounds[2] || transform.position.x < gameBounds[3])
        {
            return false;
        }
        else return true;
    }

    void FireBullet()
    {
        playerAudio.PlayOneShot(gunshotSound);
        Instantiate(bullet, copBulletSpawnObj.transform.position, copBulletSpawnObj.transform.rotation).GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * bulletForce, ForceMode.Impulse);
        reloadTimer = 0;
    }

    void DisableRagdoll()
    {
        foreach (Rigidbody rb in ragdollRB)
        {
            rb.isKinematic = true;
        }
        copRb.isKinematic = false;
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = false;
        }
        GetComponent<Collider>().enabled = true;
    }

    void EnableRagdoll()
    {
        animator.enabled = false;
        foreach (Rigidbody rb in ragdollRB)
        {
            rb.isKinematic = false;
        }
        copRb.isKinematic = true;

        foreach (Collider col in ragdollColliders)
        {
            col.enabled = true;
        }
        GetComponent<Collider>().enabled = false;

    }
}
