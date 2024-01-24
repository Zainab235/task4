using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject minePrefab;
    public GameObject bombPrefab;
    public GameObject multibombPrefab;
    public GameObject livesPrefab;
    private float spawnRange = 9.0f;
    private int objectCount;
    public int waveNum = 1;
    public GameObject enemyPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnObjectWave(waveNum);
        Instantiate(livesPrefab, GenerateSpawnPosition(), livesPrefab.transform.rotation);

        InvokeRepeating("SpawnEnemy", 15, 15.0f);

    }

    // Update is called once per frame
    void Update()
    {
        int objectCount = FindObjectsOfType<Objects>().Length;

        if (objectCount == 0)
        {
            waveNum++;
            SpawnObjectWave(waveNum);
            
        }

    }
    void SpawnEnemy()
    {

            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
       

    }
    void SpawnObjectWave(int objectSpawn)
    {
        for (int i = 0; i < objectSpawn; i++)
        { 

            Instantiate(minePrefab, GenerateSpawnPosition(), minePrefab.transform.rotation);
            Instantiate(bombPrefab, GenerateSpawnPosition(), bombPrefab.transform.rotation);
            Instantiate(multibombPrefab, GenerateSpawnPosition(), multibombPrefab.transform.rotation);

        }

    }
    private Vector3 GenerateSpawnPosition()
    {
        float spawnRangeX = Random.Range(-spawnRange, spawnRange);
        float spawnRangeZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnRangeX, 0, spawnRangeZ);
        return randomPos;
    }
}
