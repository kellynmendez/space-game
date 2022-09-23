using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] PlayerShip _playerShip;
    [SerializeField] float _timeValue = 30;
    [SerializeField] Text _timerText;
    [SerializeField] string loseText = "You ran out of time!";
    UIController _uiController = null;
    private bool _playerDead = false;

    private void Awake()
    {
        // Searching objects in scene for script of type UIController
        _uiController = FindObjectOfType<UIController>();
    }

    void Update()
    {
        if (_timeValue > 0)
        {
            _timeValue -= Time.deltaTime;
        }
        else
        {
            _playerDead = _playerShip.IsPlayerDead();
            if (!_playerDead)
            {
                EndGame();
                _timeValue = 0;
            }
        }

        DisplayTime(_timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        _timerText.text = string.Format("00:{0:00}", timeToDisplay);

        // When time is getting low, make timer more noticeable
        if (timeToDisplay < 10)
        {
            _timerText.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 1));
        }
    }

    void EndGame()
    {
        _uiController.ShowText(loseText);
        _playerShip.Kill(true);
    }
}
