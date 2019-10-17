using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AccelArea : MonoBehaviour
{
    [SerializeField]
    float _speed = 500;


    private void OnCollisionEnter(Collision other)
    {
        var collider = other.collider;
        var boat = collider.GetComponent<MyBoatController>();
        if (boat.accTimer < -1)
        {
            boat.accTimer = 0;
            boat.accDir = transform.forward;
            collider.GetComponent<Rigidbody>().AddForce(transform.forward * _speed, ForceMode.Acceleration);
        }
    }

}
