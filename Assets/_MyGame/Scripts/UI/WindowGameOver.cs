using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowGameOver : MonoBehaviour
{
    [SerializeField]
    GameObject _groupNewRecord;
    [SerializeField]
    TMPro.TextMeshProUGUI _txtNewRecord;

    public void Show(float? newrecord = null)
    {
        gameObject.SetActive(true);
        _groupNewRecord.SetActive(newrecord != null);
        if (newrecord != null)
            _txtNewRecord.text = string.Format("new record: {0}", AppString.Distance(newrecord.Value));
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
