using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    }

    private void Update()
    {
        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Game");
            _isGameOver = false;
            StopAllCoroutines();
        }
    }
    public void UpdateLives(int playerLivesLeft)
    {
        _livesDisplay.sprite = _liveSprites[playerLivesLeft];

        if (playerLivesLeft == 0)
        {
            _isGameOver = true;
            _restartText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlicker());
        }
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
