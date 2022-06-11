using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] int playerHp;
    public int pedKillCount;
    [SerializeField] int copsKilled;
    public int copCount;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI pedKilledText;
    public TextMeshProUGUI copsAliveText;


    // Start is called before the first frame update
    void Start()
    {
        playerHp = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHp < 1) Debug.Log("Player dead");
        healthText.text = $"Health: {playerHp}";
        pedKilledText.text = $"Peds Killed: {pedKillCount}";
        copsAliveText.text = $"Cop Count: {copCount}";
    }

    public void DamagePlayer(int damage)
    {
        if (playerHp > 0) playerHp -= damage;
    }
    public float[] GetBoundry()
    {
        // This method works when the ground object is a square. 
        float groundScaleZ = GameObject.Find("Ground").transform.localScale.z;
        float boundryZ = GameObject.Find("Ground").GetComponent<BoxCollider>().size.z * groundScaleZ / 2;
        float groundScaleX = GameObject.Find("Ground").transform.localScale.x;
        float boundryX = GameObject.Find("Ground").GetComponent<BoxCollider>().size.x * groundScaleX / 2;
        return new float[] { boundryZ, boundryX, -boundryZ, -boundryX };

    }

}
