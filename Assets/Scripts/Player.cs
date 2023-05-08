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
    [SerializeField]
    private float _boostedPlayerMovementSpeed = 15f;
    [SerializeField]
    private float _playerMovementSpeed;
    [SerializeField]
    private float _speedBoostMovementSpeed = 8.5f;
    private bool _isUsingThrusters = false;
    private bool _thrustersAreRecovering = false;
    private int _thrustersLeft = 100;

    [SerializeField]
    private int _thrusterRecoveryIncimentedBy = 2;
    [SerializeField]
    private int _thrustersFuelReducedBy = 2;
    [SerializeField]
    private float _thrusterFuelConsumptionRate = 0.07f;
    [SerializeField]
    private float _thrusterRecoverySpeed = 0.25f;
    [SerializeField]
    private float _speedBoostCoolDown = 3.5f;

    [SerializeField]
    private float _playerFireRate = 0.15f;
    private Vector3 _laserOffset = new Vector3(0, 0.8f, 0);
    private bool _canFireLasers = true;

    private bool _isTripleShotActive = false;
    private bool _isArcShotActive = false;

    [SerializeField]
    private bool _areShieldsActive = false;

    [SerializeField]
    private float _tripleShotCoolDown = 3.5f;

    [SerializeField]
    private float _arcShotCoolDown = 5.0f;

    [SerializeField]
    private int _playerLives = 3;

    [SerializeField]
    private GameObject _laserPrefab;

    private AudioSource _laserAudio;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _arcShotPrefab;

    [SerializeField]
    private GameObject[] _shields;
    [SerializeField]
    private int _shieldsStrength;

    [SerializeField]
    private int _currentAmmoCount = 15;
    [SerializeField]
    private int _maxAmmoCount = 15;

    private SpawnManager _spawnManager;

    private UIManager _uiManager;
    
    private AudioManager _audioManager;

    [SerializeField]
    private GameObject[] _damagedWings;
    
    // holds which random index was picked so we can select the other wing when down to 1 life.
    private int _damagedIndex;

    // Start is called before the first frame update
    void Start()
    {
        // take the current position =  new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);

        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();

        if (_audioManager == null)
        {
            Debug.LogError("Player::Start() - AudioManager is NULL");
        }

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

        if (_shields == null)
        {
            Debug.LogError("Player::Start() - Player Shields are NULL");
        }

        _laserAudio = GameObject.Find("Laser_Audio").GetComponent<AudioSource>();

        if(_laserAudio == null)
        {
            Debug.LogError("Player::Start() - Laser Audio is NULL");
        }

        if(_arcShotPrefab == null)
        {
            Debug.LogError("Player::Start() - Arc Shot Prefab is NULL");
        }

        _playerMovementSpeed = _playerBaseMovementSpeed;


        _uiManager.UpdateAmmoText(_currentAmmoCount, _maxAmmoCount);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && !_thrustersAreRecovering && !_isUsingThrusters)
        {
            _playerMovementSpeed = _boostedPlayerMovementSpeed;
            StartCoroutine(SpeedBoostCooldown());
        }

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
        } else if (_isArcShotActive)
        {
            Instantiate(_arcShotPrefab, transform.position + _laserOffset, Quaternion.identity);
        } else
        {
            Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity); 
        }

        if (_currentAmmoCount > 0)
        {
            _laserFiredCount++;
            _currentAmmoCount--;

            _laserAudio.Play();

            StartCoroutine(LaserCooldown());

            _uiManager.UpdateAmmoText(_currentAmmoCount, _maxAmmoCount);
            
        }
    }

    void AddLife()
    {
        if (_playerLives == 3)
        {
            return;
        }
        _playerLives++;

        for(var wingIndex = 0; wingIndex < _damagedWings.Length; wingIndex++)
        {
            if (_damagedWings[wingIndex].activeSelf)
            {
                _damagedWings[wingIndex].SetActive(false);
                break;
            }
        }
        _uiManager.UpdateLives(_playerLives);
    }

    IEnumerator ThrusterRecovery()
    {
        while(_thrustersLeft <= 100 && _thrustersAreRecovering)
        {
            yield return new WaitForSeconds(_thrusterRecoverySpeed);
            _thrustersLeft += _thrusterRecoveryIncimentedBy;
            _uiManager.UpdateFuelGauge(_thrustersLeft);
            if (_thrustersLeft > 100)
            {
                _thrustersLeft = 100;
                _thrustersAreRecovering = false;
            }
        }
    }

    IEnumerator UseThrusters()
    {
        while(_isUsingThrusters)
        {
            yield return new WaitForSeconds(_thrusterFuelConsumptionRate);
            _thrustersLeft -= _thrustersFuelReducedBy;
            _uiManager.UpdateFuelGauge(_thrustersLeft);
            if (_thrustersLeft < 0)
            {
                _thrustersLeft = 0;
                _isUsingThrusters = false;
            }
        }
    }
    IEnumerator LaserCooldown()
    {
        yield return new WaitForSeconds(_playerFireRate);
        _canFireLasers = true;
        StopCoroutine(LaserCooldown());
    }

    public void RemoveLife()
    {
        _uiManager.ShakeCamera();

        if (_areShieldsActive)
        {
            AdjustShields(-1);
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
            _audioManager.PlayExplosion();
            Destroy(this.gameObject);
        }
    }

    public void ActivatePowerUp(int powerupID)
    {
        switch (powerupID)
        {
            case 0:
                StartCoroutine(TripleShotCooldown());
                break;
            case 1:
                StartCoroutine(SpeedBoostCooldown());
                break;
            case 2:
                AdjustShields(1);
                break;
            case 3:
                ReloadAmmo(_maxAmmoCount);
                break;
            case 4:
                AddLife();
                break;
            case 5:
                StartCoroutine(ArcShotsCooldown());
                break;
            default:
                break;

        }
    }

    public void ReloadAmmo(int newCount)
    {
        _currentAmmoCount = _maxAmmoCount = newCount;

        _uiManager.UpdateAmmoText(_currentAmmoCount, _maxAmmoCount);
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
        _isUsingThrusters = true;

        StartCoroutine(UseThrusters());
        yield return new WaitForSeconds(_speedBoostCoolDown);
        _isUsingThrusters = false;
        _thrustersAreRecovering = true;
        
        StopCoroutine(UseThrusters());
        StartCoroutine(ThrusterRecovery());
        _playerMovementSpeed = _playerBaseMovementSpeed;
        StopCoroutine(SpeedBoostCooldown());
    }

    IEnumerator ArcShotsCooldown()
    {
        _isArcShotActive = true;
        yield return new WaitForSeconds(_arcShotCoolDown);
        _isArcShotActive = false;
        StopCoroutine(ArcShotsCooldown());
    }

    void AdjustShields(int powerChange)
    {
        // if we have full power to shields (3) and we collected another powerup
        // we don't need to add more shields, but we can get a bonus to our score.

        if (_shieldsStrength == 3 && powerChange > 0)
        {
            _score += 20;
            return;
        }

        _shieldsStrength += powerChange;

        // we can't have negative shields...
        if (_shieldsStrength < 0)
        {
            _shieldsStrength = 0;
        }

        _areShieldsActive = _shieldsStrength > 0;
        
        if (powerChange > 0)
        {
            _shields[_shieldsStrength - 1].SetActive(true);
        } 
        else 
        {
            if (_shieldsStrength == 0)
            {
                _shields[0].SetActive(false);
            }
            else
            {
                _shields[_shieldsStrength].SetActive(false);
            }
        }
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
