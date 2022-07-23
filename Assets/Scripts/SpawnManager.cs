using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private float _spawnRateMin = 1.0f;
    [SerializeField]
    private float _spawnRateMax = 3.5f;
    [SerializeField]
    private bool _spawningAllowed = true;

    [SerializeField]
    GameObject[] _powerUpPrefabs;
    

    [SerializeField]
    private float _spawnPowerUpRateMin = 3.0f;
    [SerializeField]
    private float _spawnPowerUpRateMax = 7.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (_enemyPrefab == null)
        {
            Debug.LogError("SpawnManager::Start() - Enemy Prefab is NULL");
        }

        if (_enemyContainer == null)
        {
            Debug.LogError("SpawnManager::Start() - Enemy Container is NULL");
        }

        if (_powerUpPrefabs == null)
        {
            Debug.LogError("SpawnManager::Start() - PowerUp Prefabs is NULL");
        }

        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerUp());
    }

    IEnumerator SpawnEnemies()
    {
        while (_spawningAllowed)
        {
            float randomX = Random.Range(-9.0f, 9.0f);

            Vector3 newEnemyPosition = new Vector3(randomX, 7.0f, 0);

            GameObject newEnemy = Instantiate(_enemyPrefab, newEnemyPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(Random.Range(_spawnRateMin, _spawnRateMax));
        }
    }

    IEnumerator SpawnPowerUp()
    {
        while(_spawningAllowed)
        {
            float randomRate = Random.Range(_spawnPowerUpRateMin, _spawnPowerUpRateMax);
            
            int randomPowerUpID = Random.Range(0, 3);

            GameObject powerUpPrefab = _powerUpPrefabs[randomPowerUpID];


            yield return new WaitForSeconds(randomRate);

            float randomPosX = Random.Range(-9.0f, 9.0f);

            Vector3 newPowerUpPosition = new Vector3(randomPosX, 7.0f, 0);

            GameObject newPowerUp = Instantiate(powerUpPrefab, newPowerUpPosition, Quaternion.identity);
        }
    }

    public void OnPlayerDeath(bool allowed)
    {
        _spawningAllowed = allowed;
    }
}
