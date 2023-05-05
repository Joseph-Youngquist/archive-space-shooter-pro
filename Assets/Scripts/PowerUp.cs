using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 3.0f;

    // IDs for PowerUps
    // 0 -> Triple Shot
    // 1 -> Speed Boost
    // 2 -> Shields
    // 3 -> Ammo Reload
    // 4 -> Life

    [SerializeField]
    private int _powerUpID;

    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();

        if (_audioManager == null)
        {
            Debug.LogError("PowerUp::Start() - AudioManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _movementSpeed * Time.deltaTime);

        if (transform.position.y < -5.15f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();

            if (player != null)
            {
                switch (_powerUpID)
                {
                    case 0:
                        player.ActivatePowerUp(0);
                        break;
                    case 1:
                        player.ActivatePowerUp(1);
                        break;
                    case 2:
                        player.ActivatePowerUp(2);
                        break;
                    case 3:
                        player.ActivatePowerUp(3);
                        break;
                    case 4:
                        player.ActivatePowerUp(4);
                        break;
                    default:
                        break;
                }
            }

            _audioManager.PlayPowerUp();

            Destroy(this.gameObject);
        }
    }
}
