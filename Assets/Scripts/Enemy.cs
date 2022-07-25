using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 4.0f;
    [SerializeField]
    private float _deathSpeed = 1.33f;
    [SerializeField]
    private float _deathAnimationLength = 2.5f;

    private bool _destroyed = false;

    private int _enemyValue;

    private Player _player;

    private SpawnManager _spawnManager;

    private Animator _enemyDeathAnimator;

    private AudioManager _audioManager;
    void Start()
    {
        _enemyValue = 10;

        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();

        _enemyDeathAnimator = GetComponent<Animator>();

        _player = GameObject.Find("Player").GetComponent<Player>();

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_audioManager == null)
        {
            Debug.LogError("Enemy::Start() - AudioManager is NULL");
        }

        if (_player == null)
        {
            Debug.LogError("Enemy::Start() - Player GameObject is NULL");
        }

        if (_enemyDeathAnimator == null)
        {
            Debug.LogError("Enemy::Start() - Death Animation is NULL");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("Enemy::Start() - SpawnManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _movementSpeed * Time.deltaTime);

        if (transform.position.y < -5.15f)
        {
            RespawnAtTop();
        }
    }

    void RespawnAtTop()
    {
        float randomX = Random.Range(-9.0f, 9.0f);
        transform.position = new Vector3(randomX, 7.0f, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.RemoveLife(); 
            } 
            else
            {
                Debug.LogError("Enemy::OnTriggerEnter() - player is NULL");
            }

            _destroyed = true;
        }
        
        if (other.tag == "Laser")
        {
            // destroy laser
            Destroy(other.gameObject);
            // Add to the player's score
            _player.AddToScore(_enemyValue);
            _player.AddEnemyKilled();
            _destroyed = true;
        }

        if (_destroyed)
        {
            DestructionSequence();
        }
    }

    private void DestructionSequence()
    {
        var collider = GetComponent<Collider2D>();
        collider.enabled = false;
        
        _enemyDeathAnimator.SetTrigger("OnEnemyDeath");
        
        _movementSpeed = _deathSpeed;

        _spawnManager.OnEnemyDestroyed();

        _audioManager.PlayExplosion();

        Destroy(this.gameObject, _deathAnimationLength);
    }
}
