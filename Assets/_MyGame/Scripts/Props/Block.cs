using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var boatCtrl = collision.gameObject.GetComponent<MyBoatController>();
        if (boatCtrl != null)
        {
            boatCtrl.Hurt();
        }
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<WaterSystem.BuoyantObject>().enabled = false;
    }
}
