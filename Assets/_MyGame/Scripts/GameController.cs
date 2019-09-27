using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

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
        _blackMask.color = Color.black;
        _blackMask.gameObject.SetActive(true);
    }

    private void Start()
    {
        _blackMask.DOFade(0, 0.3f);
        _allStarCount = GameObject.FindGameObjectsWithTag("star").Length;
    }

    GameState _state = GameState.Start;

    [SerializeField]
    Cinemachine.CinemachineVirtualCamera _vcamStart;
    [SerializeField]
    Cinemachine.CinemachineVirtualCamera _vcamGaming;
    [SerializeField]
    GameObject _windowTimer;
    [SerializeField]
    WindowGameOver _wndGameOver;
    [SerializeField]
    GameObject _windowHUD;
    [SerializeField]
    GameObject _windowNext;
    [SerializeField]
    RawImage _blackMask;
    [SerializeField]
    Tsunami _tsunami;
    int _allStarCount = 0;


    int _nextLevel;

    public const string _kBestRecord = "bestrecord";
    public float BestDistance
    {
        get
        {
            return PlayerPrefs.GetFloat(_kBestRecord, 0.0f);
        }
        set
        {
            PlayerPrefs.SetFloat(_kBestRecord, value);
        }
    }
    public bool HasBestRecord => PlayerPrefs.HasKey(_kBestRecord);

    public const string _kBestHit = "besthit";
    public int BestHit
    {
        get
        {
            return PlayerPrefs.GetInt(_kBestHit, 0);
        }
        set
        {
            PlayerPrefs.SetInt(_kBestHit, value);
        }
    }
    public bool HasBestHit => PlayerPrefs.HasKey(_kBestHit);

    public bool IsGaming => _state == GameState.Gaming;


    public void StartGame()
    {
        _vcamGaming.gameObject.SetActive(true);
        _windowTimer.SetActive(false);
        _StartGame();
    }

    void _StartGame()
    {
        _state = GameState.Gaming;
        _windowHUD.SetActive(true);
        var myboat = FindObjectOfType<MyBoatController>();
        _tsunami.StartMove(myboat);
    }

    public void ResetGame()
    {
        _vcamGaming.gameObject.SetActive(true);
        _windowTimer.SetActive(false);
        _state = GameState.Gaming;
        _windowHUD.SetActive(true);

    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        _state = GameState.Pause;
    }

    public void GameOver(float distance, int hit)
    {
        float? d = null;
        int? newHit = null;
        if (distance > BestDistance)
        {
            BestDistance = distance;
            d = distance;
        }
        if (hit > BestHit)
        {
            BestHit = hit;
            newHit = hit;
        }
        _wndGameOver.Show(d, newHit);
        _state = GameState.End;
        _windowHUD.SetActive(false);
        // _tsunami.StopMove();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void End(int nextlevel, float time, int hitCount, int getStarCount)
    {
        _nextLevel = nextlevel;
        Tsunami.Instance.StopMove();
        _windowHUD.SetActive(false);
        _windowNext.SetActive(true);
        _windowNext.GetComponent<WindowNext>().Show(time, hitCount, _allStarCount, getStarCount);
        _state = GameState.End;
    }

    public void Next()
    {
        if (_nextLevel >= 0 && _nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            _blackMask.DOFade(1, 0.15f).onComplete = () =>
            {
                SceneManager.LoadScene(_nextLevel);
            };
        }
        else
        {
            Debug.LogWarning("All Pass!");
        }
    }

    public void Reset()
    {
        _state = GameState.Gaming;
        _windowHUD.SetActive(false);
        _windowTimer.SetActive(true);
    }

}
