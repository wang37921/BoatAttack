using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class WindowNext : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _txtTime;
    [SerializeField]
    TextMeshProUGUI _txtStar;
    [SerializeField]
    TextMeshProUGUI _txtHit;

    [SerializeField]
    Image _cupTime;
    [SerializeField]
    Image _cupStar;
    [SerializeField]
    Image _cupHit;
    [SerializeField]
    Button _exitButton;

    [SerializeField]
    CanvasGroup[] _performances = new CanvasGroup[3];


    private void OnEnable()
    {
        foreach (var item in _performances)
        {
            item.alpha = 0;
        }
    }

    private void Start()
    {
        _exitButton.onClick.AddListener(() =>
    {
        GameController.Instance.LoadGameStart(0);
    });

    }

    public void Show(float time, int hitCount, int allStarCount, int getStarCount)
    {
        //显示结果
        var cLevelData = PlayerModel.Instance.CurrentLevelData;
        if (cLevelData == null)
        {
            Debug.LogWarning("无关卡数据！需从Start关卡运行");
            return;
        }
        // time
        if (time <= cLevelData.Time)
            _txtTime.text = time.ToString("0.0") + "/" + cLevelData.Time.ToString("0.0") + "S";
        else
            _txtTime.text = "<color=#dbdbdb>" + time.ToString("0.0") + "</color>/" + cLevelData.Time.ToString("0.0") + "S";
        _cupTime.gameObject.SetActive(time <= cLevelData.Time ? true : false);

        // star
        int needStar = (int)(allStarCount * cLevelData.Star);
        if (needStar == 0)
            _txtStar.transform.parent.gameObject.SetActive(false);
        else if (getStarCount >= needStar)
            _txtStar.text = getStarCount + "/" + needStar;
        else
            _txtStar.text = "<color=#dbdbdb>" + getStarCount + "</color>/" + needStar;
        _cupStar.gameObject.SetActive(getStarCount >= needStar || needStar == 0 ? true : false);

        // hit
        var needHit = cLevelData.Hit;
        if (needHit == 0)
            _txtHit.transform.parent.gameObject.SetActive(false);
        else if (hitCount >= needHit)
            _txtHit.text = hitCount + "/" + needHit;
        else
            _txtHit.text = "<color=#dbdbdb>" + hitCount + "</color>/" + needHit;
        _cupHit.gameObject.SetActive(hitCount >= needHit || needHit == 0 ? true : false);

        //保存数据
        var cLevel = PlayerModel.Instance.currentLevel;
        cLevel.cupTime = cLevel.cupTime || time <= cLevelData.Time;
        cLevel.cupStar = cLevel.cupStar || getStarCount >= needStar || needStar == 0;
        cLevel.cupHit = cLevel.cupHit || hitCount >= needHit || needHit == 0;

        PlayerModel.Instance.SavePlayerData();
    }

    //UI动画
    public void FadeCanvasGroup(int index)
    {
        DOTween.To(() => _performances[index].alpha, x => _performances[index].alpha = x, 1, 1).SetEase(Ease.InBack);
    }
}
