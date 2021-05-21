using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testtt : MonoBehaviour
{
    private Rigidbody rb;
    Vector3 vel = Vector3.zero;
    public float moveSpeed = 5;
    public float timeScaler = 0.2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Time.timeScale = 0.4f;
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        vel = moveInput.normalized * moveSpeed;
    }

    public void FixedUpdate()
    {
        rb.MovePosition(rb.position + vel * Time.fixedDeltaTime);
    }
}
