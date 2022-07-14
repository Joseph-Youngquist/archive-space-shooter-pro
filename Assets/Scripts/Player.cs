using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _playerMovementSpeed = 5f;

    [SerializeField]
    private GameObject _laserPrefab;


    // Start is called before the first frame update
    void Start()
    {
        // take the current position =  new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        // when the user hits the space key, spawn the laser
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
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
}
