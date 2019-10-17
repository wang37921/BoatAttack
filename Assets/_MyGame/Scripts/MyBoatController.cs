﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoatAttack.Boat;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class MyBoatController : MonoBehaviour
{
    int maxHP = 100;
    [HideInInspector]
    public int HP = 100;
    [HideInInspector]
    public int starCount = 0;

    float _timer = 0;

    int _HPExpend = 30;

    [ReadOnly]
    float _leftSeconds = 0.0f;
    float _maxSeconds = 10.0f;

    [SerializeField]
    [ReadOnly]
    bool _crashed = false;

    [SerializeField]
    public Engine _engine = null; // the engine script

    float _maxTurn = 0.4f;
    [SerializeField]
    [Range(0.5f, 1.5f)]
    float _turnSpeed = 1.0f;
    [SerializeField]
    float _accel = 1.0f;


    bool _showLines = true;
    bool _showMore = false;
    float _lineLength = 30;
    int _backFixedUpdateCount = 120;
    Vector3[] dirs1 = new Vector3[6], dirs2 = new Vector3[6];


    float _distance = 0.0f;
    float _zEnd, _zStart, _zNow;
    Rigidbody _rigidbody;

    [HideInInspector]
    public float accTimer = 0;


    bool _lastInWater = true;


    public float Distance => _distance;
    public int Hit { get; set; }
    public bool InWater => _engine.InWater;
    public float Progress => Mathf.Clamp((_zNow - _zStart) / (_zEnd - _zStart), 0f, 1f);
    public float GameTime { get => _timer; }




    public void ResetDistance()
    {
        _distance = 0.0f;
        _originPoint = transform.position;
    }
    public void RefillFuel() { _leftSeconds = _maxSeconds; }

    Vector3 _originPoint;
    Queue<MoveData> _moveDataQueue = new Queue<MoveData>();

    struct MoveData
    {
        public float accTimer;
        public Vector3 position;
        public Quaternion rotate;
        public Vector3 velocity;
        public Vector3 angularVelocity;
    }


    // Start is called before the first frame update
    void Start()
    {
        _leftSeconds = _maxSeconds;
        _rigidbody = GetComponent<Rigidbody>();
        _originPoint = transform.position;
        _zStart = transform.position.z;
        _zEnd = GameObject.Find("/End").transform.position.z;

        HP = maxHP;

        for (int i = 0; i < 6; i++)
        {
            dirs1[i] = Quaternion.Euler(0, i * 30, 0) * Vector3.right;
            dirs2[i] = Quaternion.Euler(0, i * 30 + 180, 0) * Vector3.right;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameController.Instance.IsGaming)
        {
            _timer += Time.fixedDeltaTime;
            _zNow = transform.position.z;
            accTimer -= Time.fixedDeltaTime;

            _leftSeconds -= Time.fixedDeltaTime;
            _leftSeconds = Mathf.Max(_leftSeconds, 0.0f);
            // if (_leftSeconds != 0.0f)
            {
                _engine.Accel(_accel);

                var input = Input.anyKey || Input.GetMouseButton(0);
                if (input) // Acceleration
                    _engine.Turn(_maxTurn * _turnSpeed);
                else
                    _engine.Turn(-_maxTurn * _turnSpeed);

                var dotVal = Mathf.Abs(Vector3.Dot(_rigidbody.velocity.normalized, transform.forward));
                _distance = (transform.position - _originPoint).magnitude;
            }
            // else
            //     OnFuelOver();

            // _moveDataQueue.Enqueue(new MoveData
            // {
            //     accTimer = this.accTimer,
            //     position = transform.position,
            //     rotate = transform.rotation,
            //     velocity = _rigidbody.velocity,
            //     angularVelocity = _rigidbody.angularVelocity
            // });
            // if (_moveDataQueue.Count > _backFixedUpdateCount)
            //     _moveDataQueue.Dequeue();

            FixDir();
        }
    }

    public void AddFuel(float seconds)
    {
        _leftSeconds += seconds;
    }

    public void Crash()
    {
        _crashed = true;
        GameController.Instance.GameOver(_distance, Hit);
    }

    public void ResetCrash()
    {
        _crashed = false;
    }

    // void OnFuelOver()
    // {
    //     GameController.Instance.GameOver(_distance, Hit);
    // }

    public bool HasCrash { get { return _crashed; } }

    public float LeftFuelPercent { get { return _leftSeconds / _maxSeconds; } }
    public bool HasFuel => _leftSeconds != 0.0f;


    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (GameController.Instance.IsGaming && collision.gameObject.layer == LayerMask.NameToLayer("TrackBlock"))
    //     {
    //         Hurt();
    //     }
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (GameController.Instance.IsGaming && other.gameObject.layer == LayerMask.NameToLayer("End"))
            GameController.Instance.End(_timer, Hit, starCount);

        else if (GameController.Instance.IsGaming && other.gameObject.layer == LayerMask.NameToLayer("Star"))
        {
            Destroy(other.gameObject);
            starCount++;
        }
    }

    #region 
    public void Hurt()
    {
        HP -= _HPExpend;
        if (HP <= 0)
        {
            HP = 0;
            Crash();
        }
        else
        {
            /*
            GameController.Instance.Reset();

            transform.position = ResetPosition(transform.position);
            transform.rotation = Quaternion.identity;
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            FindObjectOfType<Tsunami>().StartMove(this);
            */
            ResetByTime();
        }
    }

    /*
    List<Vector3> _midPoints;
    public Vector3 ResetPosition(Vector3 position)
    {
        _midPoints = new List<Vector3>();
        for (int i = 0; i < 6; i++)
        {
            //发射射线，获取碰撞数据
            RaycastHit hitInfo1, hitInfo2;
            if (Physics.Raycast(position, dirs1[i], out hitInfo1, _lineLength, LayerMask.GetMask("TrackBlock"))
            && Physics.Raycast(position, dirs2[i], out hitInfo2, _lineLength, LayerMask.GetMask("TrackBlock")))
            {
                _midPoints.Add(Vector3.Lerp(hitInfo1.point, hitInfo2.point, 0.5f));
            }
            else
            {
                _midPoints.Add(Vector3.zero);
            }
        }

        var points = new List<Vector3>();
        foreach (var item in _midPoints)
        {
            if (item != Vector3.zero)
                points.Add(item);
        }

        if (points.Count == 0)
        {
            Debug.LogWarning("碰撞体检测异常，无法正确重置船体位置，请检查 Line Length 和碰撞体层级“TrackBlock”");
            return new Vector3(0, position.y, position.z);
        }

        //对所有有效数据进行插值
        var resetPoint = points[0];
        for (int i = 1; i < points.Count; i++)
        {
            resetPoint = Vector3.Lerp(resetPoint, points[i], 0.5f);
        }

        return resetPoint;
    }


#if UNITY_EDITOR
    Color[] _colors = new Color[] {
            Color.red,
            Color.blue,
            Color.black,
            Color.green,
            Color.gray,
            Color.white
        };
    private void OnDrawGizmos()
    {
        if (!_showLines)
            return;
        var resetPoint = ResetPosition(transform.position);
        if (dirs1[0] != Vector3.right)
        {
            for (int i = 0; i < 6; i++)
            {
                dirs1[i] = Quaternion.Euler(0, i * 30, 0) * Vector3.right;
                dirs2[i] = Quaternion.Euler(0, i * 30 + 180, 0) * Vector3.right;
            }
        }
        Gizmos.color = Color.red;
        for (int i = 0; i < 6; i++)
        {
            if (_showMore)
            {
                Gizmos.color = _colors[i];
                Gizmos.DrawSphere(_midPoints[i], 1);
            }
            Gizmos.DrawLine(transform.position, transform.position + dirs1[i] * _lineLength);
            Gizmos.DrawLine(transform.position, transform.position + dirs2[i] * _lineLength);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(resetPoint, 1);
    }
#endif
 */
    #endregion

    public void ResetByTime()
    {
        if (_moveDataQueue.Count > 1)
        {
            var moveData = _moveDataQueue.Dequeue();
            accTimer = moveData.accTimer;
            transform.position = moveData.position;
            transform.rotation = moveData.rotate;
            _rigidbody.velocity = moveData.velocity;
            _rigidbody.angularVelocity = moveData.angularVelocity;
            _moveDataQueue.Clear();
        }
        FindObjectOfType<Tsunami>().StartMove();

        GameController.Instance.Reset();
    }


    [HideInInspector]
    public Vector3? accDir = null;

    [SerializeField]
    bool _fixRotate = true;

    [SerializeField]
    [Range(0, 0.25f)]
    float _maxFixIntensity = 0.1f;
    [SerializeField]
    float _fixDuration = 3;
    float _fixIntensity = 0.1f;
    bool _needFix = false;


    void FixDir()
    {
        if (!_fixRotate)
            return;

        // 加速后第一次入水
        if (accDir != null && InWater && !_lastInWater)
        {
            _fixIntensity = _maxFixIntensity;
            _needFix = true;
            DOTween.To(() => { return _fixIntensity; }, (intensity) => { _fixIntensity = intensity; }, 0f, _fixDuration).onComplete += () =>
            {
                _needFix = false;
                accDir = null;
            };
        }
        _lastInWater = InWater;

        if (_needFix && accDir != null)
            Rotate2Dir(accDir.Value, _fixIntensity);

        // Debug.Log(_fixIntensity);
    }

    void Rotate2Dir(Vector3 dir, float intensity)
    {
        Vector3 targetDir = new Vector3(dir.x, 0, dir.z);
        Vector3 nowDir = transform.forward;//new Vector3(transform.forward.x, 0, transform.forward.z);
        var toDir = Vector3.Lerp(nowDir, targetDir, intensity);

        var rotate = Quaternion.FromToRotation(nowDir, toDir);
        transform.Rotate(rotate.eulerAngles, Space.World);
    }

}
