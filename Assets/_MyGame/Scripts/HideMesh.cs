using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HideMesh : MonoBehaviour
{
    private void Awake()
    {
        // GetComponent<MeshRenderer>().material.SetColor("_BaseColor", new Color(0, 0, 0, 0));
        GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0);
    }
}
