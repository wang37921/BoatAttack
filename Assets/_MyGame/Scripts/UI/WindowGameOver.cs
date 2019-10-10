using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowGameOver : MonoBehaviour
{
    [SerializeField]
    GameObject _groupNewRecord;
    [SerializeField]
    TMPro.TextMeshProUGUI _txtNewRecord;
    [SerializeField]
    TMPro.TextMeshProUGUI _txtNewHit;

    public void Show(float? newrecord = null, int? newHit = null)
    {
        gameObject.SetActive(true);

        // _groupNewRecord.SetActive(newrecord != null || newHit != null);
        // _txtNewRecord.gameObject.SetActive(newrecord != null);
        // if (newrecord != null)
        //     _txtNewRecord.text = string.Format("new dist: {0}", AppString.Distance(newrecord.Value));
        // _txtNewHit.gameObject.SetActive(newHit != null);
        // if (newHit != null)
        //     _txtNewHit.text = string.Format("new hit: {0}", AppString.Hit(newHit.Value));
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
