using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopShooter
{
    public class PlayerControllerShooter : MonoBehaviour
    {
        Vector3 velocity;
        Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Move(Vector3 velocity)
        {
            this.velocity = velocity;
        }

        public void LookAtMouse(Vector3 lookPoint)
        {
            Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
            transform.LookAt(heightCorrectedPoint);
        }

        public void FixedUpdate()
        {
            //Debug.Log(velocity * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }
}