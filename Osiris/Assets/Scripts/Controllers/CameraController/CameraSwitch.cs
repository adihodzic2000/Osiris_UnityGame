using Osiris.Controllers.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Osiris.CameraController
{
    public class CameraSwitch : MonoBehaviour
    {
        [SerializeField] 
        private GameObject Camera1;

        [SerializeField] 
        private GameObject Camera2;

        [SerializeField] 
        private GameObject Camera3;

        [SerializeField]
        private InputManagement Manager;

        private void Start()
        {
            Manager.OnC1 += OnC1;
            Manager.OnC2 += OnC2;
            Manager.OnC3 += OnC3;
        }
        private void OnC1()
        {
            Camera1.SetActive(true);
            Camera2.SetActive(false);
            Camera3.SetActive(false);
        }
        private void OnC2()
        {
            Camera1.SetActive(false);
            Camera2.SetActive(true);
            Camera3.SetActive(false);
        }
        private void OnC3()
        {
            Camera1.SetActive(false);
            Camera2.SetActive(false);
            Camera3.SetActive(true);
        }
    }
}
