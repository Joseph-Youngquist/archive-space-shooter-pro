using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcShot : MonoBehaviour
{
    [SerializeField]
    private float _arcAngle;
    
    [SerializeField]
    private float _arcShotLaserMovementSpeed = 15f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _arcShotLaserMovementSpeed * Time.deltaTime);

        if (transform.position.y > 7.1f)
        {

            if (this.transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }
}
