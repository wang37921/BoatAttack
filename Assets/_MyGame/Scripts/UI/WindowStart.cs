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
    [SerializeField]
    GameObject _windowLevel;



    private void Start()
    {
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        gameObject.SetActive(false);
        _windowLevel.SetActive(true);
    }
}
