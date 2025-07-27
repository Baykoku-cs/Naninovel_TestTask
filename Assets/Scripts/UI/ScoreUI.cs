using Naninovel;
using Naninovel.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private CustomUI _ui;

    private int _showedScore;
    private int _wishedScore;
    private int _updateSpeed = 50;
    
    private void Start()
    {
        ScoreService scoreService = Engine.GetService<ScoreService>();
        if (scoreService != null)
        {
            scoreService.OnScoreChanged += ScoreService_OnScoreChanged;
            _wishedScore = scoreService.GetScore();
            _showedScore = _wishedScore;
            _scoreText.text = _showedScore.ToString();
        }
        else
        {
            Debug.LogError("ScoreService is not available.");
        }
    }

    private float _timer;
    private float _calmTimer;
    private float _calmTimerMax = 1.5f;
    private void Update()
    {
        if (_showedScore != _wishedScore)
        {
            _ui.Show();
            var audioManager = Engine.GetService<IAudioManager>();
            if (!audioManager.IsSfxPlaying("score"))
            {
                audioManager.PlaySfxAsync("score");
            }
            _timer += Time.deltaTime;
            if (_timer >= 1f / _updateSpeed)
            {
                _timer = 0f;
                if (_showedScore < _wishedScore)
                {
                    _showedScore++;
                }
                else if (_showedScore > _wishedScore)
                {
                    _showedScore--;
                }
                _scoreText.text = _showedScore.ToString();
                _calmTimer = 0f;
            }
        }
        if (_calmTimer < _calmTimerMax)
        {
            _calmTimer += Time.deltaTime;
            if (_calmTimer >= _calmTimerMax)
            {
                _ui.Hide();
            }
        }
       
    }

    private void ScoreService_OnScoreChanged(int newScore, bool showUi)
    {
        if (showUi)
        {
            _wishedScore = newScore;
            _timer = 0f;
        }
        else
        {
            _wishedScore = newScore;
            _showedScore = _wishedScore;
            _scoreText.text = _showedScore.ToString();
        }
    }
}
