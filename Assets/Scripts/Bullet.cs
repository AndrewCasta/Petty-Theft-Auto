using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float[] gameBounds;

    // Start is called before the first frame update
    void Start()
    {
        gameBounds = GameObject.Find("GameManager").GetComponent<GameManager>().GetBoundry();

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > gameBounds[0] || transform.position.x > gameBounds[1] || transform.position.z < gameBounds[2] || transform.position.x < gameBounds[3])
        {
            Destroy(gameObject);
        }
    }
}
