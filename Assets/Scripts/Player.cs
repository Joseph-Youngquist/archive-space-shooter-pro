using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _playerMovementSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        // take the current position =  new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementVector = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(movementVector * _playerMovementSpeed * Time.deltaTime);

        // prevent the player from moving higher than 0 on the y position
        // prevent the player from moving below than -3.75 on the y position

        // wrap the player on the left and right bounds when player position is
        // less than -9.2 or greater than 9.2

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.75f)
        {
            transform.position = new Vector3(transform.position.x, -3.75f, 0);
        }

        if (transform.position.x <= -9.2f)
        {
            transform.position = new Vector3(9.19f, transform.position.y, 0);
        } else if (transform.position.x >= 9.2f)
        {
            transform.position = new Vector3(-9.19f, transform.position.y, 0);
        }
    }
}
