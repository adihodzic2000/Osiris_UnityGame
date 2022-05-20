using System;
using UnityEngine;
namespace Osiris.Controllers.Core
{
    public class InputManagement : MonoBehaviour
    {
        //SerializedField
        [SerializeField] private KeyCode keyN1;
        [SerializeField] private KeyCode keyN2;
        [SerializeField] private KeyCode keyC1;
        [SerializeField] private KeyCode keyC2;
        [SerializeField] private KeyCode keyC3;
        //Actions
        public Action<float, float> OnInputChanged;
        public Action<float, float> OnRotate;
        public Action<float> OnRotateCamera1;
        public Action OnBrake;
        public Action OnC1;
        public Action OnC2;
        public Action OnC3;
        private void Update()
        {
            if (Input.GetKey(keyC1))
                OnC1?.Invoke();
            if (Input.GetKey(keyC2))
                OnC2?.Invoke();
            if (Input.GetKey(keyC3))
                OnC3?.Invoke();
        }
        private void FixedUpdate()
        {
            OnInputChanged?.Invoke(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (Input.GetKey(keyN1))
                OnRotate?.Invoke(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            if (Input.GetKey(keyN2))
                OnBrake?.Invoke();
        }
        private void LateUpdate()
        {
            OnRotateCamera1?.Invoke(Input.GetAxis("Mouse X"));
        }
    }
}
