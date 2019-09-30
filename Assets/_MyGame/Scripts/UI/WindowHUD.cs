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

    [SerializeField]
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
    float _tsunamiWarningDistance = 30.0f;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!_myBoat.HasCrash)
        {
            if (_myBoat.LeftFuelPercent == 0.0f)
            {
                for (var i = 0; i != _drops.Length; ++i)
                {
                    var c = _drops[i].color;
                    c.a = _invalidDropAlpha;
                    _drops[i].color = c;
                }
            }
            else
            {
                var leftDropCount = Mathf.CeilToInt(_myBoat.LeftFuelPercent * _drops.Length);
                for (var i = 0; i != _drops.Length; ++i)
                {
                    var c = _drops[i].color;
                    c.a = i < leftDropCount ? _validDropAlpha : _invalidDropAlpha;
                    _drops[i].color = c;
                }
            }
        }

        _txtDistance.text = AppString.Distance(_myBoat.Distance);
        _txtHit.text = AppString.Hit(_myBoat.Hit);
        _txtStar.text = _myBoat.starCount.ToString();
        _txtTime.text = AppString.GameTime(_myBoat.GameTime);

        var distance = Mathf.Abs(_tsunami.transform.position.z - _myBoat.transform.position.z);
        _txtTsunami.text = AppString.Distance(distance);
        _txtTsunami.color = Color.Lerp(Color.white, Color.red, 1.0f - distance / _tsunamiWarningDistance);

        _nowHP.fillAmount = (float)_myBoat.HP / _myBoat.maxHP;
        _nowProgress.fillAmount = _myBoat.Progress;
    }
}
