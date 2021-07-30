using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private float speed = 1;
	[SerializeField] private float rotateSpeed = 1;

    void Update()
    {
		Move();
		RotateCam();
    }

	private void Move()
	{
		if (Input.GetKey(KeyCode.W))
		{
			transform.position += transform.forward * speed;
		}
		if (Input.GetKey(KeyCode.S))
		{
			transform.position += -transform.forward * speed;
		}
		if (Input.GetKey(KeyCode.A))
		{
			transform.position += -transform.right * speed;
		}
		if (Input.GetKey(KeyCode.D))
		{
			transform.position += transform.right * speed;
		}
	}

	private void RotateCam()
	{
		if (Input.GetKey(KeyCode.Mouse1))
		{
			transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * speed, Input.GetAxis("Mouse X") * speed, 0));
			var X = transform.rotation.eulerAngles.x;
			var Y = transform.rotation.eulerAngles.y;
			transform.rotation = Quaternion.Euler(X, Y, 0);
		}
	}
}
