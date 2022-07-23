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

    [SerializeField]
    private int _powerUpID;

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
                        player.ActivatePowerUp("Triple_Shot");
                        break;
                    case 1:
                        player.ActivatePowerUp("Speed_Boost");
                        break;
                    case 2:
                        player.ActivatePowerUp("Shields");
                        break;
                    default:
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
