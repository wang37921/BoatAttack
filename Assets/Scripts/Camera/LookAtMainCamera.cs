using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMainCamera : MonoBehaviour
{
    enum Axis
    {
        x,
        y,
        z,
        none
    }

    [SerializeField]
    Axis _targetAxis = Axis.x;
    [SerializeField]
    Vector3 _offset = Vector3.zero;

    Transform _cameraPoint;
    Quaternion _rotation;
    bool _enable = false;

    private void Awake()
    {
        _cameraPoint = Camera.main.transform;

        LookAt(_cameraPoint);

        transform.Rotate(_offset, Space.Self);
    }
    void FixedUpdate()
    {
        if (!_enable)
            return;

        LookAt(_cameraPoint);

        transform.Rotate(_offset, Space.Self);
    }

    private void OnBecameVisible()
    {
        _enable = true;
    }

    private void OnBecameInvisible()
    {
        _enable = false;
    }

    void LookAt(Transform target)
    {
        var dir = Vector3.Normalize(target.position - transform.position);

        var rotate = Quaternion.FromToRotation(transform.forward, dir);

        if (_targetAxis != Axis.x && _targetAxis != Axis.none)
            rotate.eulerAngles = new Vector3(0, rotate.eulerAngles.y, rotate.eulerAngles.z);
        if (_targetAxis != Axis.y && _targetAxis != Axis.none)
            rotate.eulerAngles = new Vector3(rotate.eulerAngles.x, 0, rotate.eulerAngles.z);
        if (_targetAxis != Axis.z && _targetAxis != Axis.none)
            rotate.eulerAngles = new Vector3(rotate.eulerAngles.x, rotate.eulerAngles.y, 0);

        transform.Rotate(rotate.eulerAngles, Space.World);
    }
}
