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
    CanvasGroup[] _performances = new CanvasGroup[3];


    private void OnEnable()
    {
        foreach (var item in _performances)
        {
            item.alpha = 0;
        }
    }

    public void Show(float time, int hitCount, int allStarCount, int getStarCount)
    {
        //显示结果
        // time
        if (time <= LevelEnd.Instance.time)
            _txtTime.text = time.ToString("0.0") + "/" + LevelEnd.Instance.time.ToString("0.0") + "S";
        else
            _txtTime.text = "<color=#dbdbdb>" + time.ToString("0.0") + "</color>/" + LevelEnd.Instance.time.ToString("0.0") + "S";
        _cupTime.gameObject.SetActive(time <= LevelEnd.Instance.time ? true : false);

        // star
        int needStar = (int)(allStarCount * LevelEnd.Instance.star);
        if (getStarCount >= needStar)
            _txtStar.text = getStarCount + "/" + needStar;
        else
            _txtStar.text = "<color=#dbdbdb>" + getStarCount + "</color>/" + needStar;
        _cupStar.gameObject.SetActive(getStarCount >= needStar ? true : false);

        // hit
        if (hitCount >= LevelEnd.Instance.hit)
            _txtHit.text = hitCount + "/" + LevelEnd.Instance.hit;
        else
            _txtHit.text = "<color=#dbdbdb>" + hitCount + "</color>/" + LevelEnd.Instance.hit;
        _cupHit.gameObject.SetActive(hitCount >= LevelEnd.Instance.hit ? true : false);

        //保存数据
    }

    //UI动画
    public void FadeCanvasGroup(int index)
    {
        DOTween.To(() => _performances[index].alpha, x => _performances[index].alpha = x, 1, 1).SetEase(Ease.InBack);
    }
}
