using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelArea : MonoBehaviour
{
    [SerializeField]
    float _speed = 25;


    private void OnCollisionStay(Collision other)
    {
        other.collider.GetComponent<Rigidbody>().AddForce(transform.forward * _speed, ForceMode.Acceleration);
    }

}
