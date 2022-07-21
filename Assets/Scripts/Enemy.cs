using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 4.0f;

    private bool _destroyed = false;

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
            // damage the player
            Player player = other.transform.GetComponent<Player>();
            
            if (player != null)
            {
                player.RemoveLife(); 
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
            _destroyed = true;
        }

        if (_destroyed)
        {
            Destroy(this.gameObject);
        }
    }
}
