using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidgetKilometer : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI _txtMesh;

    MyBoatController _myboat;

    private void Start()
    {
        _myboat = FindObjectOfType<MyBoatController>();
    }

    // Update is called once per frame
    void Update()
    {
        _txtMesh.text = AppString.Distance(_myboat.Distance);
    }
}
