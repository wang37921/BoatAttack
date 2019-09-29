using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelArea : MonoBehaviour
{
    [SerializeField]
    float _speed;

    MyBoatController _boat;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_boat)
        {
            // Debug.Log(_speed);
            // _boat.GetComponent<Rigidbody>().velocity = transform.forward * _speed;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        _boat = other.collider.GetComponent<MyBoatController>();
        if (_boat)
            _boat.GetComponent<Rigidbody>().AddForce(transform.forward * _speed, ForceMode.Acceleration);
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.collider.GetComponent<MyBoatController>())
            _boat = null;
    }

}
