using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    List<Vector3> spawnPoints = new List<Vector3>();

    [SerializeField] GameObject pedObject;
    [SerializeField] GameObject copObject;

    float spawnTime = 0.5f;
    float spawnTimer;

    public int pedCount;

    [SerializeField] int minPeds = 5;
    [SerializeField] int minCops = 0;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 0;

        // Get spawn points
        foreach (GameObject spawnObject in GameObject.FindGameObjectsWithTag("npc spawn"))
        {
            Vector3 spawnPoint = spawnObject.transform.position + new Vector3(0, 5f, 0);
            spawnPoints.Add(spawnPoint);
        }
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        //if (startSpawnComplete) SpawnLogic();
        SpawnLogic();
    }

    void SpawnNPCs(GameObject npc, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPosition = spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
            Instantiate(npc, randomPosition, pedObject.transform.rotation * Quaternion.Euler(0, 90 * Random.Range(0, 3), 0));
        }
        if (npc == pedObject) pedCount++;
        if (npc == copObject) gameManager.copCount++;
        spawnTimer = 0;
    }



    void SpawnLogic()
    {
        spawnTimer += Time.deltaTime;
        if (gameManager.pedKillCount >= 1) minCops = 1;
        if (gameManager.pedKillCount >= 5)
        {
            minPeds = 10;
            minCops = 2;
        }
        if (gameManager.pedKillCount >= 10)
        {
            minPeds = 15;
            minCops = 3;
        }
        if (gameManager.pedKillCount >= 15)
        {
            minPeds = 20;
            minCops = 5;
        }
        if (spawnTimer > spawnTime)
        {
            if (pedCount < minPeds) SpawnNPCs(pedObject, 1);
            if (gameManager.copCount < minCops) SpawnNPCs(copObject, 1);
        }
    }
}
