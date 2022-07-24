using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 33.3f;
    [SerializeField]
    private float _destroyAnimationLength = 2.5f;

    private SpawnManager _spawnManager;
    private Animator _destroyAnim;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Asteroid::Start() - SpawnManager is NULL");
        }

        _destroyAnim = GetComponent<Animator>();
        if (_destroyAnim == null)
        {
            Debug.LogError("Asteroid::Start() - Destruction Animation is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _destroyAnim.SetTrigger("OnDestroy");
            Destroy(other.gameObject);
            Destroy(this.gameObject, _destroyAnimationLength);

            _spawnManager.StartWave();
        }
    }
}
