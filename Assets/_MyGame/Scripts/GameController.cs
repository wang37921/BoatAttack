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

    public const string _kBestRecord = "bestrecord";
    public float BestDistance {
        get {
            return PlayerPrefs.GetFloat(_kBestRecord, 0.0f);
        }
        set {
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
    }

    public void Restart()
    {
        _state = GameState.Gaming;
        _wndGameOver.Hide();
        var myboat = FindObjectOfType<MyBoatController>();
        myboat.ResetDistance();
        myboat.RefillFuel();
        myboat.ResetCrash();
        myboat.Hit = 0;
        _windowHUD.SetActive(true);

        // center to track.
        var resetPos = myboat.transform.position;
        resetPos.x = 0.0f;
        myboat.transform.position = resetPos;
        myboat.transform.rotation = Quaternion.identity;
    }
}
