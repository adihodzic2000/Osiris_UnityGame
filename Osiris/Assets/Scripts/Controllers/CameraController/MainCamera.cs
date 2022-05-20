using Osiris.Controllers.Core;
using UnityEngine;

namespace Osiris.CameraController
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField]
        private GameObject obj;

        [SerializeField] 
        private InputManagement Manager;

        void Start()
        {
            Manager.OnRotate += OnRotate;
        }


        private void OnRotate(float MouseX, float MouseY)
        {
            this.transform.LookAt(obj.transform.position);
            transform.RotateAround(obj.transform.position, Vector3.up, MouseX * 5);
        }
       
    }
}
