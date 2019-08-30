using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tsunami : MonoBehaviour
{
    public static Tsunami Instance { get; private set; }
    public static Tsunami NewInstance()
    {
        var go = new GameObject("Tsunami", typeof(Tsunami));
        DontDestroyOnLoad(go);
        return Instance;
    }
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField]
    float _speed = 1.3f;
    [SerializeField]
    float _tsunamiInitDistance = 30.0f;

    Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void StartMove(MyBoatController myboat)
    {
        var boatPos = myboat.transform.position;
        boatPos.x = 0;
        transform.position = boatPos + Vector3.back * _tsunamiInitDistance;
        _rigidbody.velocity = Vector3.forward * _speed;
    }

    public void StopMove()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        var boatCtrl = other.gameObject.GetComponent<MyBoatController>();
        if (boatCtrl != null)
            boatCtrl.Crash();
    }

}
