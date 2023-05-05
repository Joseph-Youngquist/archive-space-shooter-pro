using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _asteroidPrefab;

    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private float _spawnRateMin = 1.0f;
    [SerializeField]
    private float _spawnRateMax = 3.5f;
    [SerializeField]
    private bool _spawningAllowed = false;
    [SerializeField]
    private bool _enemySpawningAllowed = false;

    private Player _player;

    [SerializeField]
    private int _waveNumber = 0;
    [SerializeField]
    private int _waveMaxEnemies;
    [SerializeField]
    private int _numberOfEnemiesSpawned = 0;
    [SerializeField]
    private int _numberOfDestroyedEnemies = 0;
    [SerializeField]
    private int _baseWaveLimit = 10;

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

        if (_player == null)
        {
            //Debug.LogError("SpawnManager::Start() - Player is NULL");
            _player = GameObject.Find("Player").GetComponent<Player>();
        }

        if (_powerUpPrefabs == null)
        {
            Debug.LogError("SpawnManager::Start() - PowerUp Prefabs is NULL");
        }

        if (_asteroidPrefab == null)
        {
            Debug.LogError("SpawnManager::Start() - Asteroid Prefab is NULL");
        }

        SpawnAsteroid();
    }
    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(1.0f);
        while (_enemySpawningAllowed)
        {
            _numberOfEnemiesSpawned++;

            float randomX = Random.Range(-9.0f, 9.0f);

            Vector3 newEnemyPosition = new Vector3(randomX, 7.0f, 0);

            GameObject newEnemy = Instantiate(_enemyPrefab, newEnemyPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(Random.Range(_spawnRateMin, _spawnRateMax));

            // when the wave limit of enemies have spawned turn off spawning of more enemies.
            _enemySpawningAllowed = _numberOfEnemiesSpawned < _waveMaxEnemies;
        }
    }

    void SpawnAsteroid()
    {
        Instantiate(_asteroidPrefab, new Vector3(0, 4.0f, 0), Quaternion.identity);
    }
    public void OnEnemyDestroyed()
    {
        _numberOfDestroyedEnemies++;

        // if all enemies have spawned and been destroyed then start new wave sequence.
        if (_numberOfDestroyedEnemies == _waveMaxEnemies)
        {
            StopAllCoroutines();

            SpawnAsteroid();
        }
    }

    IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(1.0f);
        while (_spawningAllowed)
        {
            float randomRate = Random.Range(_spawnPowerUpRateMin, _spawnPowerUpRateMax);
            
            int randomPowerUpID = Random.Range(0, 5);

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
        StopAllCoroutines();
    }

    public void StartWave()
    {
        _waveNumber++;

        _spawningAllowed = true;
        _enemySpawningAllowed = true;
        
        _numberOfEnemiesSpawned = 0;
        _numberOfDestroyedEnemies = 0;
        
        _waveMaxEnemies = _waveNumber * _baseWaveLimit;

        int _maxAmmoForWave = _waveMaxEnemies + _waveNumber;

        if (_maxAmmoForWave < 15)
        {
            _maxAmmoForWave = 15;
        }
        _player.ReloadAmmo(_maxAmmoForWave);

        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerUp());
    }
}
