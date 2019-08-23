using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowStart : MonoBehaviour
{
    [SerializeField]
    GameObject _groupBestRecord;
    [SerializeField]
    TMPro.TextMeshProUGUI _txtBestRecord;
    [SerializeField]
    TMPro.TextMeshProUGUI _txtBestHit;

    // Start is called before the first frame update
    void Start()
    {
        _groupBestRecord.SetActive(GameController.Instance.HasBestRecord || GameController.Instance.HasBestHit);
        _txtBestRecord.text = AppString.Distance(GameController.Instance.BestDistance);
        _txtBestHit.text = AppString.Hit(GameController.Instance.BestHit);
    }
}
