using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _explosionAudio;
    private AudioSource _powerUpAudio;

    private void Start()
    {
        _explosionAudio = GameObject.Find("Explosion_Audio").GetComponent<AudioSource>();
        _powerUpAudio = GameObject.Find("PowerUp_Audio").GetComponent<AudioSource>();

        if (_explosionAudio == null)
        {
            Debug.LogError("AudioManager::Start() - Explosion Audio is NULL");
        }

        if (_powerUpAudio == null)
        {
            Debug.LogError("AudioManager::Start() - PowerUp Audio is NULL");
        }

    }
    public void PlayExplosion()
    {
        _explosionAudio.Play();
    }

    public void PlayPowerUp()
    {
        _powerUpAudio.Play();
    }
}
