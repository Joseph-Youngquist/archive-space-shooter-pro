using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserMovementSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _laserMovementSpeed * Time.deltaTime);

        if (transform.position.y > 7.1f)
        {
            Destroy(this.gameObject);
        }
    }
}
