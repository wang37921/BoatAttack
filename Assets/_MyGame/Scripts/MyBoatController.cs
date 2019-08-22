using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoatAttack.Boat;


public class MyBoatController : MonoBehaviour
{
    [ReadOnly]
    [SerializeField]
    float _leftSeconds = 0.0f;
    [SerializeField]
    float _maxSeconds = 10.0f;

    [SerializeField]
    [ReadOnly]
    bool _crashed = false;

    [SerializeField]
    Engine _engine = null; // the engine script

    [SerializeField]
    float _maxTurn = 0.4f;
    [SerializeField]
    float _accel = 3.0f;

    float _distance = 0.0f;
    Rigidbody _rigidbody;

    public float Distance => _distance;

    public void ResetDistance()
    {
        _distance = 0.0f;
        _originPoint = transform.position;
    }
    public void RefillFuel() { _leftSeconds = _maxSeconds; }

    Vector3 _originPoint;

    public bool Stall = true;

    // Start is called before the first frame update
    void Start()
    {
        _leftSeconds = _maxSeconds;
        _rigidbody = GetComponent<Rigidbody>();
        _originPoint = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameController.Instance.IsGaming)
        {
            _leftSeconds -= Time.fixedDeltaTime;
            _leftSeconds = Mathf.Max(_leftSeconds, 0.0f);
            if (_leftSeconds != 0.0f)
            {
                _engine.Accel(_accel);

                var input = Input.anyKey || Input.GetMouseButton(0);
                if (input) // Acceleration
                    _engine.Turn(_maxTurn);
                else
                    _engine.Turn(-_maxTurn);

                var dotVal = Mathf.Abs(Vector3.Dot(_rigidbody.velocity.normalized, transform.forward));
                _distance = (transform.position - _originPoint).magnitude;
            }
            else
                OnFuelOver();
        }
    }

    public void AddFuel(float seconds)
    {
        _leftSeconds += seconds;
    }

    public void Crash()
    {
        _crashed = true;
        GameController.Instance.GameOver(_distance);
    }

    public void ResetCrash()
    {
        _crashed = false;
    }

    void OnFuelOver()
    {
        GameController.Instance.GameOver(_distance);
    }

    public bool HasCrash { get { return _crashed; } }

    public float LeftFuelPercent { get { return _leftSeconds / _maxSeconds; } }
    public bool HasFuel => _leftSeconds != 0.0f;
}
