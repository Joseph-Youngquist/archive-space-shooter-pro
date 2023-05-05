using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private Image _livesDisplay;
    [SerializeField]
    private TMP_Text _gameOverText;
    [SerializeField]
    private TMP_Text _restartText;
    [SerializeField]
    private TMP_Text _gameStatsText;
    [SerializeField]
    private TMP_Text _ammoText;

    [SerializeField]
    private Sprite[] _liveSprites;

    private bool _isGameOver = false;

    [SerializeField]
    private GameManager _gameManager;

    private Color _red = new Color(1f, 0.25f, 0.25f);
    private Color _white = new Color(1f, 1f, 1f);

    private bool _isOutOfAmmo = false;

    // Start is called before the first frame update
    void Start()
    {
        if(_ammoText == null)
        {
            Debug.LogError("UIManager::Start() - Ammo Text is NULL");
        }
        if (_scoreText == null)
        {
            Debug.LogError("UIManager::Start() - Score Text Mesh is NULL.");
        }

        if (_livesDisplay == null)
        {
            Debug.LogError("UIManager::Start() - Lives Display Image is NULL");
        }

        if (_gameOverText == null)
        {
            Debug.LogError("UIManager::Start() - Game Over Text is NULL");
        }

        if (_restartText == null)
        {
            Debug.LogError("UIManager::Start() - Restart Game Text is NULL");
        }

        if (_gameStatsText == null)
        {
            Debug.LogError("UIManager::Start() - Game Stats Text is NULL");
        }

        if (_gameManager == null)
        {
            Debug.LogError("UIManager::Start() - GameManager is NULL");
        }
    }

    public void UpdateLives(int playerLivesLeft)
    {
        _livesDisplay.sprite = _liveSprites[playerLivesLeft];

        if (playerLivesLeft == 0)
        {
            GameOverSequence();
        }
    }

    public void UpdateGameStats(int laserCount, int enemiesKilled)
    {
        float accuracy = 0.0f;

        if (laserCount > 0)
        {
            accuracy = ((float) enemiesKilled / (float) laserCount) * 100f;
        }
        
        string statsText = "Lasers Fired:\t" + laserCount.ToString() + " times\n";
        statsText += "Enemies Killed:\t" + enemiesKilled + "\n";
        statsText += "Laser Accuracy:\t" + accuracy.ToString("n2") + "%";
        _gameStatsText.text = statsText;
    }

    void GameOverSequence()
    {
        _isGameOver = true;
        
        _restartText.gameObject.SetActive(true);
        _gameStatsText.gameObject.SetActive(true);

        StartCoroutine(GameOverFlicker());
        _gameManager.GameOver();
    }
    public void UpdateScoreText(int newScore)
    {
        _scoreText.text = "Score: " + newScore;
    }

    public void UpdateAmmoText(int currentAmmoCount, int maxAmmoCount)
    {
        _ammoText.text = "Ammo: " + currentAmmoCount.ToString() + "/" + maxAmmoCount.ToString();

        if (currentAmmoCount == 0)
        {
            _isOutOfAmmo = true;
            _ammoText.color = _red;
            StartCoroutine(OutOfAmmoFlicker());
        }
        else
        {
            _isOutOfAmmo = false;
            StopCoroutine(OutOfAmmoFlicker());
            _ammoText.color = _white;
            _ammoText.gameObject.SetActive(true);
        }
    }
    IEnumerator OutOfAmmoFlicker()
    {
        while(_isOutOfAmmo)
        {
            _ammoText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            _ammoText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator GameOverFlicker()
    {
        while (_isGameOver)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
