using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WindowHUD : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI _txtDistance;

    [SerializeField]
    TMPro.TextMeshProUGUI _txtHit;

    [SerializeField]
    TMPro.TextMeshProUGUI _txtTsunami;
    [SerializeField]
    TMPro.TextMeshProUGUI _txtStar;
    [SerializeField]
    TMPro.TextMeshProUGUI _txtTime;

    Image[] _drops;

    [SerializeField]
    Image _nowHP;
    [SerializeField]
    Image _nowProgress;
    [SerializeField]
    Button _pauseButton;

    [SerializeField]
    float _invalidDropAlpha = 0.5f;
    [SerializeField]
    float _validDropAlpha = 1.0f;
    [SerializeField]
    float _fontMaxScale = 15f;

    float _fontSize;


    MyBoatController _myBoat;
    Tsunami _tsunami;


    // Start is called before the first frame update
    void Start()
    {
        _myBoat = FindObjectOfType<MyBoatController>();
        _tsunami = FindObjectOfType<Tsunami>();
        _pauseButton.onClick.AddListener(() =>
        {
            GameController.Instance.Pause();
        });

        _fontSize = _txtTsunami.fontSize;
    }

    // Update is called once per frame
    void Update()
    {
        // if (!_myBoat.HasCrash)
        // {
        //     if (_myBoat.LeftFuelPercent == 0.0f)
        //     {
        //         for (var i = 0; i != _drops.Length; ++i)
        //         {
        //             var c = _drops[i].color;
        //             c.a = _invalidDropAlpha;
        //             _drops[i].color = c;
        //         }
        //     }
        //     else
        //     {
        //         var leftDropCount = Mathf.CeilToInt(_myBoat.LeftFuelPercent * _drops.Length);
        //         for (var i = 0; i != _drops.Length; ++i)
        //         {
        //             var c = _drops[i].color;
        //             c.a = i < leftDropCount ? _validDropAlpha : _invalidDropAlpha;
        //             _drops[i].color = c;
        //         }
        //     }
        // }

        _txtDistance.text = AppString.Distance(_myBoat.Distance);
        _txtHit.text = AppString.Hit(_myBoat.Hit);
        _txtStar.text = _myBoat.starCount.ToString();
        _txtTime.text = AppString.GameTime(_myBoat.GameTime);

        _txtTsunami.text = AppString.Distance(GameController.Instance.TsunamiDistance);
        _txtTsunami.color = GameController.Instance.TsunamiDistance < GameController.Instance.nearDistance ? Color.Lerp(Color.red, Color.white, 0.3f) : Color.white;
        // Color.Lerp(Color.white, Color.red, 1.0f - GameController.Instance.TsunamiDistance / _tsunamiWarningDistance);
        _txtTsunami.fontSize = _fontSize + _fontMaxScale * Mathf.Clamp(1.0f - GameController.Instance.TsunamiDistance / GameController.Instance.nearDistance, 0, 1);

        // _nowHP.fillAmount = (float)_myBoat.HP / _myBoat.maxHP;
        _nowProgress.fillAmount = _myBoat.Progress;
    }
}
