using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour
{
    [SerializeField]
    float _minSeconds = 5.0f;
    [SerializeField]
    float _maxSeconds = 10.0f;

    [ReadOnly]
    [SerializeField]
    float _seconds;

    // Start is called before the first frame update
    void Start()
    {
        _seconds = Random.Range(_minSeconds, _maxSeconds);
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     var boatCtrl = other.gameObject.GetComponent<MyBoatController>();
    //     if (boatCtrl != null)
    //     {
    //         boatCtrl.AddFuel(_seconds);
    //         gameObject.SetActive(false);
    //     }
    // }
}
