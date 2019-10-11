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

    private void Start() {
        _exitButton.onClick.AddListener(() =>
    {
    GameController.Instance.LoadGameStart(0);
});

    }

    public void Show(float time, int hitCount, int allStarCount, int getStarCount)
    {
        //显示结果
        // time
        if (time <= PlayerModel.Instance.CurrentLevelData.Time)
            _txtTime.text = time.ToString("0.0") + "/" + PlayerModel.Instance.CurrentLevelData.Time.ToString("0.0") + "S";
        else
            _txtTime.text = "<color=#dbdbdb>" + time.ToString("0.0") + "</color>/" + PlayerModel.Instance.CurrentLevelData.Time.ToString("0.0") + "S";
        _cupTime.gameObject.SetActive(time <= PlayerModel.Instance.CurrentLevelData.Time ? true : false);

        // star
        int needStar = (int)(allStarCount * PlayerModel.Instance.CurrentLevelData.Star);
        if (getStarCount >= needStar)
            _txtStar.text = getStarCount + "/" + needStar;
        else
            _txtStar.text = "<color=#dbdbdb>" + getStarCount + "</color>/" + needStar;
        _cupStar.gameObject.SetActive(getStarCount >= needStar ? true : false);

        // hit
        if (hitCount >= PlayerModel.Instance.CurrentLevelData.Hit)
            _txtHit.text = hitCount + "/" + PlayerModel.Instance.CurrentLevelData.Hit;
        else
            _txtHit.text = "<color=#dbdbdb>" + hitCount + "</color>/" + PlayerModel.Instance.CurrentLevelData.Hit;
        _cupHit.gameObject.SetActive(hitCount >= PlayerModel.Instance.CurrentLevelData.Hit ? true : false);

        //保存数据
        PlayerModel.Instance.currentLevel.cupTime = PlayerModel.Instance.currentLevel.cupTime || time <= PlayerModel.Instance.CurrentLevelData.Time;
        PlayerModel.Instance.currentLevel.cupStar = PlayerModel.Instance.currentLevel.cupStar || getStarCount >= needStar;
        PlayerModel.Instance.currentLevel.cupHit = PlayerModel.Instance.currentLevel.cupHit || hitCount >= PlayerModel.Instance.CurrentLevelData.Hit;
    }

    //UI动画
    public void FadeCanvasGroup(int index)
    {
        DOTween.To(() => _performances[index].alpha, x => _performances[index].alpha = x, 1, 1).SetEase(Ease.InBack);
    }
}
