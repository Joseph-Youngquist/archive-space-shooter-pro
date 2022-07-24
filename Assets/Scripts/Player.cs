using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _score;
    [SerializeField]
    private int _laserFiredCount;
    [SerializeField]
    private int _enemiesKilled;

    [SerializeField]
    private float _playerBaseMovementSpeed = 5f;
    private float _playerMovementSpeed;
    [SerializeField]
    private float _speedBoostMovementSpeed = 8.5f;
    [SerializeField]
    private float _speedBoostCoolDown = 3.5f;

    [SerializeField]
    private float _playerFireRate = 0.15f;
    private Vector3 _laserOffset = new Vector3(0, 0.8f, 0);
    private bool _canFireLasers = true;

    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _areShieldsActive = false;

    [SerializeField]
    private float _tripleShotCoolDown = 3.5f;

    [SerializeField]
    private int _playerLives = 3;

    [SerializeField]
    private GameObject _laserPrefab;
    
    [SerializeField]
    private GameObject _tripleShotPrefab;
    


    private SpawnManager _spawnManager;
    private GameObject _playerShields;

    private UIManager _uiManager;

    [SerializeField]
    private GameObject[] _damagedWings;
    
    // holds which random index was picked so we can select the other wing when down to 1 life.
    private int _damagedIndex;

    // Start is called before the first frame update
    void Start()
    {
        // take the current position =  new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);

        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("Player::Start() - uiManager is NULL");
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Player::Start() - Spawn Manager is NULL");
        }

        _playerShields = this.transform.GetChild(0).gameObject;

        if (_playerShields == null)
        {
            Debug.LogError("Player::Start() - Player Shields are NULL");
        }

        _playerMovementSpeed = _playerBaseMovementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        
        // when the user hits the space key, spawn the laser
        if (Input.GetButtonDown("Fire1") && _canFireLasers)
        {
            FireLaser();
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementVector = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(movementVector * _playerMovementSpeed * Time.deltaTime);

        // Keep the player's ship within a clamped range of 0 max and -3.75 min.
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.75f, 0), 0);

        /*
         * wrap the player on the left and right bounds when player position is
         * less than -9.3 or greater than 9.3
         * we multiply the x position by negative value very close to -1 but not -1
         * if we simply multiply by -1 then we could have an edge case where we're
         * setting the postion at an equals boundry of either -9.3 or 9.3 and the 
         * ship will "bounce" between the left and right sides of the game window.
         * 
         */
        if (transform.position.x <= -9.3f || transform.position.x >= 9.3f)
        {
            transform.position = new Vector3(transform.position.x * -0.9999f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        
        _canFireLasers = false;
        
        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position + _laserOffset, Quaternion.identity);
        } else
        {
            Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity); 
        }
        
        _laserFiredCount++;
        
        StartCoroutine(LaserCooldown());
    }

    IEnumerator LaserCooldown()
    {
        yield return new WaitForSeconds(_playerFireRate);
        _canFireLasers = true;
        StopCoroutine(LaserCooldown());
    }

    public void RemoveLife()
    {
        if (_areShieldsActive)
        {
            ActivateShields(false);
            return;
        }

        _playerLives--;

        if (_playerLives == 2)
        {
            _damagedIndex = Random.Range(0, 2);
            _damagedWings[_damagedIndex].SetActive(true);
        }

        if (_playerLives == 1)
        {
            if (_damagedIndex == 0)
            {
                _damagedWings[1].SetActive(true);
            } else
            {
                _damagedWings[0].SetActive(true);
            }
        }

        
        _uiManager.UpdateLives(_playerLives);

        if (_playerLives < 1)
        {
            _spawnManager.OnPlayerDeath(false);
            _uiManager.UpdateGameStats(_laserFiredCount, _enemiesKilled);
            Destroy(this.gameObject);
        }
    }

    public void ActivatePowerUp(string powerupName)
    {
        switch (powerupName)
        {
            case "Triple_Shot":
                StartCoroutine(TripleShotCooldown());
                break;
            case "Speed_Boost":
                StartCoroutine(SpeedBoostCooldown());
                break;
            case "Shields":
                ActivateShields(true);
                break;
            default:
                break;

        }
    }

    IEnumerator TripleShotCooldown()
    {
        _isTripleShotActive = true;
        yield return new WaitForSeconds(_tripleShotCoolDown);
        _isTripleShotActive = false;
        StopCoroutine(TripleShotCooldown());
    }
    
    IEnumerator SpeedBoostCooldown()
    {
        _playerMovementSpeed = _speedBoostMovementSpeed;
        yield return new WaitForSeconds(_speedBoostCoolDown);
        _playerMovementSpeed = _playerBaseMovementSpeed;
        StopCoroutine(SpeedBoostCooldown());
    }

    void ActivateShields(bool status)
    {
        _areShieldsActive = status;
        _playerShields.SetActive(_areShieldsActive);
    }

    public void AddToScore(int value)
    {
        _score += value;
        _uiManager.UpdateScoreText(_score);
    }

    public void AddEnemyKilled()
    {
        _enemiesKilled++;
    }
}
