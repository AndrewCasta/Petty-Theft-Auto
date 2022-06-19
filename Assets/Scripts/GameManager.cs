using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isGameActive;
    [SerializeField] GameObject player;

    [SerializeField] int playerHp;
    public int pedKillCount;
    public int copsKilledCount;
    public int copCount;

    SpawnManager spawnManager;

    [SerializeField] GameObject startScreen;

    [SerializeField] GameObject HUD;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI pedKilledTextHUD;
    [SerializeField] TextMeshProUGUI copsAliveTextHUD;

    [SerializeField] GameObject gameOverScreen;
    [SerializeField] TextMeshProUGUI pedKilledTextSummary;
    [SerializeField] TextMeshProUGUI copsKilledTextSummary;

    // Start is called before the first frame update
    void Start()
    {
        isGameActive = false;
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = $"Health: {playerHp}";
        pedKilledTextHUD.text = $"Peds Killed: {pedKillCount}";
        copsAliveTextHUD.text = $"Cop Count: {copCount}";
    }

    public void StartGame()
    {
        HUD.SetActive(true);
        startScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        ResetGame();
    }

    public void DamagePlayer(int damage)
    {
        if (playerHp > 0) playerHp -= damage;
        if (playerHp < 1) GameOver();
    }

    void GameOver()
    {
        isGameActive = false;
        HUD.SetActive(false);
        startScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        pedKilledTextSummary.text = $"Pedestrian Kills: {pedKillCount}";
        copsKilledTextSummary.text = $"Cop Kills: {copsKilledCount}";
    }

    private void ResetGame()
    {
        isGameActive = true;
        playerHp = 5;
        pedKillCount = 0;
        copsKilledCount = 0;
        copCount = 0;
        spawnManager.pedCount = 0;
        player.transform.position = new Vector3(0, 1.17f, 0);
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("npc");
        foreach (GameObject npc in npcs) Destroy(npc);
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
