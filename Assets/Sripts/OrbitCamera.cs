using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float rotSpeed = 1.5f;

    private float _rotY;
    private float _rotX;
    private Vector3 _offset;

	void Start ()
    {
        _rotY = this.transform.eulerAngles.y;
        _offset = target.position - this.transform.position;
	}
	
	void LateUpdate ()
    {
        _rotY -= Input.GetAxis("Horizontal") * this.rotSpeed;
        Quaternion rotation = Quaternion.Euler(0, _rotY, 0);
        transform.position = this.target.position - (rotation * _offset);
        transform.LookAt(this.target);
	}
}