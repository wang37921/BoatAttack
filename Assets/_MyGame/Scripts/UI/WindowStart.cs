using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowStart : MonoBehaviour
{
    [SerializeField]
    GameObject _groupBestRecord;
    [SerializeField]
    TMPro.TextMeshProUGUI _txtBestRecord;

    // Start is called before the first frame update
    void Start()
    {
        _groupBestRecord.SetActive(GameController.Instance.HasBestRecord);
        _txtBestRecord.text = string.Format("Best: {0}", AppString.Distance(GameController.Instance.BestDistance));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
