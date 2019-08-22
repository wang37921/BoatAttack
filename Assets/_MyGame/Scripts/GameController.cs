using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Start, 
    Gaming, 
    Pause, 
    End, 
}

[DefaultExecutionOrder(-3000)]
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public static GameController NewInstance()
    {
        var go = new GameObject("GameController", typeof(GameController));
        DontDestroyOnLoad(go);
        return Instance;
    }
    private void Awake()
    {
        Instance = this;
    }

    GameState _state = GameState.Start;

    [SerializeField]
    Cinemachine.CinemachineVirtualCamera _vcamStart;
    [SerializeField]
    Cinemachine.CinemachineVirtualCamera _vcamGaming;
    [SerializeField]
    GameObject _windowStart;
    [SerializeField]
    WindowGameOver _wndGameOver;
    [SerializeField]
    GameObject _windowHUD;

    float _bestDistance = 0.0f;

    public bool IsGaming => _state == GameState.Gaming;


    public void StartGame()
    {
        _vcamGaming.gameObject.SetActive(true);
        _windowStart.SetActive(false);
        StopCoroutine("_StartGame");
        StartCoroutine("_StartGame");
    }

    IEnumerator _StartGame()
    {
        yield return new WaitForSeconds(2.0f);
        _state = GameState.Gaming;
        _windowHUD.SetActive(true);
    }

    // public void Pause()
    // {
    //     Time.timeScale = 0.0f;
    //     _state = GameState.Pause;
    // }

    public void GameOver(float distance)
    {
        float? d = null;
        if (distance > _bestDistance)
        {
            _bestDistance = distance;
            d = distance;
        }
        _wndGameOver.Show(d);
        _state = GameState.End;
        _windowHUD.SetActive(false);
    }

    public void Restart()
    {
        _state = GameState.Gaming;
        _wndGameOver.Hide();
        var myboat = FindObjectOfType<MyBoatController>();
        myboat.ResetDistance();
        myboat.RefillFuel();
        myboat.ResetCrash();
        _windowHUD.SetActive(true);
    }
}
