using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;

    public float runForce = 20f;
    public float sprintMultiplyer = 2f;

    private Vector3 movement;

    public float[] gameBounds;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        // Setting player boundry width ground box collider
        SetBoundry();
    }

    // Update is called once per frame
    void Update()
    {
        // Base movement Vector for WASD input
        movement = new Vector3 (Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Sprint speed multiplier
        if (Input.GetKey(KeyCode.LeftShift)) {
            movement *= sprintMultiplyer;
        }
    }

    private void FixedUpdate()
    {
        // 3D physics movement
        playerRb.MovePosition(transform.position + (movement * runForce * Time.deltaTime));

        // Setting player boundry width ground box collider
        PlayerMoveBoundry();
    }

    void SetBoundry()
    {
        float groundScaleZ = GameObject.Find("Ground").transform.localScale.z;
        float boundryZ = GameObject.Find("Ground").GetComponent<BoxCollider>().size.z * groundScaleZ / 2;
        float groundScaleX = GameObject.Find("Ground").transform.localScale.x;
        float boundryX = GameObject.Find("Ground").GetComponent<BoxCollider>().size.x * groundScaleX / 2;
        gameBounds = new float[] { boundryZ, boundryX, -boundryZ, -boundryX };
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
