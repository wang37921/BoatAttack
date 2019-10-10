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

        if (collider.GetComponent<MyBoatController>().accTimer < -1)
        {
            collider.GetComponent<MyBoatController>().accTimer = 0;
            collider.GetComponent<Rigidbody>().AddForce(transform.forward * _speed, ForceMode.Acceleration);
        }
    }

}
