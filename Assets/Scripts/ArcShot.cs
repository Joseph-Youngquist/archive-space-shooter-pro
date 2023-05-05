using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcShot : MonoBehaviour
{
    [SerializeField]
    private float _arcAngle;
    
    [SerializeField]
    private float _arcShotLaserMovementSpeed = 15f;

    void Start()
    {
        //float angle = Random.Range(-spreadAngle, spreadAngle);
        Quaternion rotation = Quaternion.AngleAxis(_arcAngle, Vector3.forward);
        transform.rotation = rotation;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * _arcShotLaserMovementSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        // transform.Translate(Vector3.up * _arcShotLaserMovementSpeed * Time.deltaTime);

        if (transform.position.y > 9.1f)
        {

            if (this.transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }
}
