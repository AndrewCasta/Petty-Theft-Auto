using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedMovement : MonoBehaviour
{
    Rigidbody pedRb;
    [SerializeField] float moveSpeed;

    [SerializeField] float bulletCollisionForce;

    [SerializeField] bool isAlive;

    SpawnManager spawnManager;
    GameManager gameManager;
    float[] gameBounds;

    // Start is called before the first frame update
    void Start()
    {
        pedRb = GetComponent<Rigidbody>();
        isAlive = true;

        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Setting player boundry width ground box collider
        gameBounds = gameManager.GetBoundry();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isAlive && !InBoundry())
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAlive)
        {
            PlayerMoveBoundry();
            pedRb.MovePosition(transform.position + (transform.forward * moveSpeed * Time.deltaTime));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            isAlive = false;
            spawnManager.pedCount--;
            gameManager.pedKillCount++;
            pedRb.angularVelocity = Vector3.zero;
            pedRb.velocity = Vector3.zero;
            pedRb.freezeRotation = false;
            Vector3 direction = (collision.GetContact(0).point - transform.position).normalized;
            pedRb.AddForce(-direction * bulletCollisionForce, ForceMode.Impulse);
        }
    }

    void PlayerMoveBoundry()
    {
        if (transform.position.z > gameBounds[0] || transform.position.x > gameBounds[1] || transform.position.z < gameBounds[2] || transform.position.x < gameBounds[3])
        {
            transform.Rotate(0f, 180f, 0f, Space.Self);
        }

    }
    bool InBoundry()
    {
        if (transform.position.z > gameBounds[0] || transform.position.x > gameBounds[1] || transform.position.z < gameBounds[2] || transform.position.x < gameBounds[3])
        {
            return false;
        }
        else return true;
    }
}
