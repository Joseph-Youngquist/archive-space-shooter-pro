using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeControl : MonoBehaviour
{
    [SerializeField]
    public float shakeIntensity = 0.5f;
    [SerializeField]
    public float shakeSpeed = 1.0f;
    [SerializeField]
    public float shakeDuration = 0.5f;

    private Vector3 originalPos;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShakeCamera()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float randomX = Mathf.PerlinNoise(Time.time * shakeSpeed, 0.0f);
            float randomY = Mathf.PerlinNoise(0.0f, Time.time * shakeSpeed);
            float offsetX = (randomX * 2.0f - 1.0f) * shakeIntensity;
            float offsetY = (randomY * 2.0f - 1.0f) * shakeIntensity;
            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0.0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
