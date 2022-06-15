using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Vector3 movement;
    GameManager gameManager;
    float[] gameBounds;

    [SerializeField] float runForce = 20f;
    [SerializeField] float sprintMultiplyer = 2f;
    [SerializeField] float turnSpeed;

    [SerializeField] GameObject bullet;
    [SerializeField] float bulletForce;

    GameObject playerBulletSpawnPoint;

    AudioSource playerAudio;
    [SerializeField] AudioClip gunshotSound;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        gameBounds = gameManager.GetBoundry();
        playerBulletSpawnPoint = transform.Find("Bullet Point").gameObject;

        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") || Input.GetButtonDown("Fire1")) FireBullet();
    }

    private void FixedUpdate()
    {
        // Base movement Vector for WASD input
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Sprint speed multiplier
        if (Input.GetKey(KeyCode.LeftShift)) {
            movement *= sprintMultiplyer;
        }

        // 3D physics movement
        playerRb.MovePosition(transform.position + (movement * runForce * Time.deltaTime));

        // Setting player boundry width ground box collider
        PlayerMoveBoundry();

        // Smooth turning (rb)
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement);
            Quaternion rotatation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
            playerRb.MoveRotation(rotatation);
        }

        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            gameManager.DamagePlayer(1);
        }
    }

    void FireBullet()
    {
        playerAudio.PlayOneShot(gunshotSound);
        Instantiate(bullet, playerBulletSpawnPoint.transform.position, playerBulletSpawnPoint.transform.rotation).GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * bulletForce, ForceMode.Impulse);
    }

    void PlayerMoveBoundry()
    {
        if (transform.position.z > gameBounds[0] - 0.999){
            transform.position = new Vector3(transform.position.x, transform.position.y, gameBounds[0] - 1);
        } else if (transform.position.x > gameBounds[1] - 0.999) {
            transform.position = new Vector3(gameBounds[1] - 1, transform.position.y, transform.position.z);
        } else if (transform.position.z < gameBounds[2] + 0.999) {
            transform.position = new Vector3(transform.position.x, transform.position.y, gameBounds[2] + 1);
        } else if (transform.position.x < gameBounds[3] + 0.999) {
            transform.position = new Vector3(gameBounds[3] + 1, transform.position.y, transform.position.z);
        }
    }
}