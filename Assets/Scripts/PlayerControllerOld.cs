using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOld : MonoBehaviour
{
    private Rigidbody playerRb;
    private Vector3 movement;
    float[] gameBounds;

    [SerializeField] float runForce = 20f;
    [SerializeField] float sprintMultiplyer = 2f;
    [SerializeField] float turnSpeed;
    Vector3 eulerAngleVelocity;

    [SerializeField] float bulletForce;

    GameObject playerBulletSpawnObj;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        eulerAngleVelocity = new Vector3(0, turnSpeed, 0);

        gameBounds = GameObject.Find("GameManager").GetComponent<GameManager>().GetBoundry();
        playerBulletSpawnObj = transform.Find("Bullet Point").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            GameObject bullet = BulletPool.SharedInstance.GetPooledBullet();
            if (bullet != null)
            {
                bullet.transform.position =
                    playerBulletSpawnObj.transform.position;
                bullet.transform.rotation = playerBulletSpawnObj.transform.rotation;
                bullet.SetActive(true);
                bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulletForce, ForceMode.Impulse);
            }
        }
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

        // Smooth turning (rb)
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement);
            playerRb.MoveRotation(Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.deltaTime));
        }

        // Setting player boundry width ground box collider
        PlayerMoveBoundry();
    }

    void FireBullet()
    {
        
    }

    void PlayerMoveBoundry()
    {
        if (transform.position.z > gameBounds[0]){
            transform.position = new Vector3(transform.position.x, transform.position.y, gameBounds[0]);
        } else if (transform.position.x > gameBounds[1]) {
            transform.position = new Vector3(gameBounds[1], transform.position.y, transform.position.z);
        } else if (transform.position.z < gameBounds[2]) {
            transform.position = new Vector3(transform.position.x, transform.position.y, gameBounds[2]);
        } else if (transform.position.x < gameBounds[3]) {
            transform.position = new Vector3(gameBounds[3], transform.position.y, transform.position.z);
        }
    }
}


// Smooth turning (transform)
//if (movement != Vector3.zero) {
//    Quaternion toRotation = Quaternion.LookRotation(movement);
//    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
//}