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
    float _speed = 12f;
    [SerializeField]
    float _tsunamiInitDistance = 30.0f;
    [Header("起点到终点间的速度")]
    public AnimationCurve _speedCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(1, 1) });
    [SerializeField]
    float _moveSpeedLerp = 0.07f;
    MyBoatController _myboat;

    bool _needMove = false;

    Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _myboat = FindObjectOfType<MyBoatController>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity,
        _needMove ?
        Vector3.forward * _speed * _speedCurve.Evaluate(_myboat.Progress)
        : Vector3.zero
        , _moveSpeedLerp);

    }

    public void StartMove()
    {
        var boatPos = _myboat.transform.position;
        boatPos.x = 0;
        transform.position = boatPos + Vector3.back * _tsunamiInitDistance;
        _needMove = true;
    }

    public void StopMove()
    {
        _needMove = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var boatCtrl = other.gameObject.GetComponent<MyBoatController>();
        if (boatCtrl != null)
            boatCtrl.Crash();
    }

}
