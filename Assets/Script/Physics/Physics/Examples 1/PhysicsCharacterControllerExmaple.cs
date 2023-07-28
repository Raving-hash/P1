using BlueNoah.Math.FixedPoint;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlueNoah.PhysicsEngine
{
    public class PhysicsCharacterControllerExmaple : MonoBehaviour
    {
        public Vector3 force;
        [HideInInspector]
        public FixedPointVector3 orientation;
        [SerializeField]
        Button jumpBtn;
        [SerializeField]
        Camera mainCamera;
        public FixedPointCharacterController actor;
        FixedPoint64 moveSpeed = 5;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                actor.AddForce(new FixedPointVector3(force));
            }
        }
        private void FixedUpdate()
        {
            var movement = FixedPointVector3.zero;
            
            actor.Move(movement);
        }
    }
}