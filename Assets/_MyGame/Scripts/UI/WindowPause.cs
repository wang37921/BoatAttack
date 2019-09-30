using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowPause : MonoBehaviour
{
    [SerializeField]
    Button _continueButton;
    [SerializeField]
    Button _exitButton;

    private void Start()
    {
        _continueButton.onClick.AddListener(() =>
        {
            GameController.Instance.Continue();
        });

        _exitButton.onClick.AddListener(() =>
        {
            GameController.Instance.LoadGame(0);
        });
    }
}
