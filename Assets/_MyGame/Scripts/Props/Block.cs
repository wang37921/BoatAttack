using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var boatCtrl = collision.gameObject.GetComponent<MyBoatController>();
        if (boatCtrl != null)
        {
            boatCtrl.Crash();
        }
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<WaterSystem.BuoyantObject>().enabled = false;
    }
}
