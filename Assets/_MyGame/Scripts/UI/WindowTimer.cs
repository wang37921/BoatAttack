using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class WindowTimer : MonoBehaviour
{
    [SerializeField]
    float _startTime = 3;
    [SerializeField]
    float _timeScale = 0.2f;
    [SerializeField]
    GameController _gameController;
    [SerializeField]
    TextMeshProUGUI _title;

    float _timer;

    private void OnEnable()
    {
        _timer = Time.unscaledTime;

        Time.timeScale = _timeScale;
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, _startTime).SetEase(Ease.InExpo);
    }

    private void Update()
    {
        _title.text = Mathf.Ceil(_startTime - (Time.unscaledTime - _timer)).ToString();
        if (Time.unscaledTime - _timer > _startTime)
        {
            if (GameController.Instance.IsGaming)
            {
                _gameController.ResetGame();
            }
            else if (GameController.Instance.IsPausing)
            {
                _gameController.ContinueGame();
            }
            else
            {
                _gameController.StartGame();
            }
        }
    }
}
