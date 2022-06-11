using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopMovement : MonoBehaviour
{
    Rigidbody copRb;
    [SerializeField] bool isAlive;
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;

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

    // Start is called before the first frame update
    void Start()
    {
        copRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        isAlive = true;
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
            copRb.angularVelocity = Vector3.zero;
            copRb.velocity = Vector3.zero;
            copRb.freezeRotation = false;
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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.deltaTime);

        // Chasing player & shooting range check
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        if (distanceToPlayer > distanceToShoot)
        {
            readyToShoot = false;
            copRb.MovePosition(transform.position + (transform.forward * moveSpeed * Time.deltaTime));
        }
        else
        {
            readyToShoot = true;
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
