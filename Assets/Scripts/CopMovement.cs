using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopMovement : MonoBehaviour
{
    Rigidbody thisRb;
    [SerializeField] bool isAlive;
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;

    [SerializeField] float distanceToShoot;
    [SerializeField] bool readyToShoot;
    [SerializeField] float reloadTime;
    float reloadTimer;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletForce;

    GameObject copBulletSpawnObj;

    float distanceToPlayer;
    GameObject player;
    Vector3 directionToPlayer;

    GameManager gameManager;
    float[] gameBounds;

    Animator animator;
    AudioSource playerAudio;
    [SerializeField] AudioClip gunshotSound;

    // Start is called before the first frame update
    void Start()
    {
        thisRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        isAlive = true;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        gameBounds = gameManager.GetBoundry();
        copBulletSpawnObj = transform.Find("Bullet Point").gameObject;
        playerAudio = GetComponent<AudioSource>();
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
            thisRb.MovePosition(transform.position + (transform.forward * moveSpeed * Time.deltaTime));
            // transform.position = transform.position + (transform.forward * moveSpeed * Time.deltaTime);

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
}
