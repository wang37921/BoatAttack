using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoatAttack.Boat;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class MyBoatController : MonoBehaviour
{
    public int maxHP = 100;
    [HideInInspector]
    public int HP = 100;
    [HideInInspector]
    public int starCount = 0;

    [SerializeField]
    int _HPExpend = 30;

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
    float _accel = 1.0f;


    [SerializeField]
    bool _showLines = true;
    [SerializeField]
    bool _showMore = false;
    [SerializeField]
    float _lineLength = 30;
    [SerializeField]
    int _backFixedUpdateCount = 120;
    Vector3[] dirs1 = new Vector3[6], dirs2 = new Vector3[6];


    float _distance = 0.0f;
    Rigidbody _rigidbody;

    public float Distance => _distance;
    public int Hit { get; set; }
    public bool InWater => _engine.InWater;

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

        _moveDataQueue.Enqueue(new MoveData
        {
            position = transform.position,
            rotate = transform.rotation,
            velocity = _rigidbody.velocity,
            angularVelocity = _rigidbody.angularVelocity
        });
        if (_moveDataQueue.Count > _backFixedUpdateCount)
            _moveDataQueue.Dequeue();

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

    void OnFuelOver()
    {
        GameController.Instance.GameOver(_distance, Hit);
    }

    public bool HasCrash { get { return _crashed; } }

    public float LeftFuelPercent { get { return _leftSeconds / _maxSeconds; } }
    public bool HasFuel => _leftSeconds != 0.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (GameController.Instance.IsGaming && collision.gameObject.layer == LayerMask.NameToLayer("TrackBlock"))
        {
            Hurt();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var levelEnd = other.GetComponent<LevelEnd>();
        if (GameController.Instance.IsGaming && levelEnd)
            GameController.Instance.End(levelEnd.NextLevel, starCount);
        else if (GameController.Instance.IsGaming && other.gameObject.layer == LayerMask.NameToLayer("Star"))
        {
            Destroy(other.gameObject);
            starCount++;
        }
    }

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
            // GameController.Instance.Reset();

            // transform.position = ResetPosition(transform.position);
            // transform.rotation = Quaternion.identity;
            // transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            // transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            // FindObjectOfType<Tsunami>().StartMove(this);

            ResetByTime();
        }
    }
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

    public void ResetByTime()
    {
        if (_moveDataQueue.Count > 1)
        {
            var moveData = _moveDataQueue.Dequeue();
            transform.position = moveData.position;
            transform.rotation = moveData.rotate;
            _rigidbody.velocity = moveData.velocity;
            _rigidbody.angularVelocity = moveData.angularVelocity;
            _moveDataQueue.Clear();
        }
        FindObjectOfType<Tsunami>().StartMove(this);
    }
}
