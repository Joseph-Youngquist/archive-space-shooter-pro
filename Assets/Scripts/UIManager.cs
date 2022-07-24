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
    private Sprite[] _liveSprites;

    private bool _isGameOver = false;

    [SerializeField]
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
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

    void GameOverSequence()
    {
        _isGameOver = true;
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
        _gameManager.GameOver();
    }
    public void UpdateScoreText(int newScore)
    {
        _scoreText.text = "Score: " + newScore;
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
