using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windsurfer : MonoBehaviour
{
    [SerializeField]
    float _maxForce = 1.0f;
    [SerializeField]
    float _minForce = 0.3f;
    [SerializeField]
    BoatAttack.RandomHue _boatMesh;

    [ReadOnly]
    [SerializeField]
    bool _destroied = false;

    float _force;
    Rigidbody _rb;


    // Start is called before the first frame update
    void Start()
    {
        _force = Random.Range(_minForce, _maxForce);
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_destroied && _boatMesh.enable)
            _rb.AddForce(transform.forward * _force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_destroied)
        {
            _destroied = true;
            _rb.useGravity = true;

            var boatCtrl = collision.gameObject.GetComponent<MyBoatController>();
            if (boatCtrl != null)
                boatCtrl.Hit += 1;
        }
    }
}
