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

    Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.IsGaming)
            _rigidbody.velocity = Vector3.forward * _speed;
        else
            _rigidbody.velocity = Vector3.zero;

    }
}
