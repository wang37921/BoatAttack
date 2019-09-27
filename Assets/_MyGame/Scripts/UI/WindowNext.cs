using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class WindowNext : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _txtStar;

    [SerializeField]
    CanvasGroup[] _performances = new CanvasGroup[3];

    [SerializeField]
    Image _cupTime;
    [SerializeField]
    

    private void OnEnable() {
        foreach (var item in _performances)
        {
            item.alpha = 0;
        }
    }

    public void Show(float time, int hitCount, int allStarCount, int getStarCount)
    {
        //显示结果
        _txtStar.text = getStarCount + "/" + allStarCount;

        //保存数据
    }

    //UI动画
    public void FadeCanvasGroup(int index)
    {
        DOTween.To(() => _performances[index].alpha, x => _performances[index].alpha = x, 1, 1).SetEase(Ease.InBack);
    }
}
