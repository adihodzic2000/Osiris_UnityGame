using Osiris.Controllers.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Osiris.CameraController
{
    public class CameraRotate : MonoBehaviour
    {
        [SerializeField] 
        private Transform PlayerTransform;
        
        [SerializeField] 
        private Vector3 CameraOffset;

        [SerializeField]
        [Range(0.01f, 1.0f)] private float SmoothFactor = 0.5F;

        [SerializeField]
        private bool LookAtPlayer = false;

        [SerializeField]
        private bool RotateAroundPlayer = true;

        [SerializeField] 
        private float RotationSpeed = 5.0f;

        [SerializeField]
        private InputManagement InputManager;

        public InputManagement OnRotateCamera { get; private set; }

        void Start()
        {
            CameraOffset = transform.position - PlayerTransform.position;
            InputManager.OnRotateCamera1 += OnRotateCamera1;
        }
        private void OnRotateCamera1(float MouseX)
        {
            if (RotateAroundPlayer)
            {
                Quaternion camTurnAngle =
                    Quaternion.AngleAxis(MouseX * RotationSpeed, Vector3.up);

                CameraOffset = camTurnAngle * CameraOffset;
            }

            Vector3 newPos = PlayerTransform.position + CameraOffset;

            transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

            if (LookAtPlayer || RotateAroundPlayer)
                transform.LookAt(PlayerTransform);
        }
       
    }
}
