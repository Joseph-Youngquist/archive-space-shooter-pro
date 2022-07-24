using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 4.0f;

    private bool _destroyed = false;

    private int _enemyValue;

    private Player _player;

    void Start()
    {
        _enemyValue = 10;

        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Enemy::Start() - Player GameObject is NULL");
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
            Destroy(this.gameObject);
        }
    }
}
