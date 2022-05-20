using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Osiris.Controllers.CarController
{
    public class BotController : MonoBehaviour
    {
        [SerializeField] [ReorderableList] private List<GameObject> Wheels;
        [SerializeField] private int HowMuchPositions;
        [SerializeField] private int MaxSpeed;
        [SerializeField] private int CurveSpeed;
        [SerializeField] private LayerMask GroundLayer;
        [SerializeField] private GameObject MassFront;
        [SerializeField] private GameObject MassRear;
        [SerializeField] private float Y;
        private NavMeshAgent NavAgent;
        private NavMeshPath Path { get; set; }
        private Rigidbody BotRigidbody;
        private MeshCollider Mesh;
        public GameObject Target { get; set; }
        private Vector3 Current { get; set; }
        private Vector3 LastRotation { get; set; }
        private Vector3 NextRotation { get; set; }
        private bool Ine { get; set; }
        private bool IsCarGroundedRearWheels { get; set; }
        private bool IsCarGroundedFrontWheels { get; set; }
        private bool IsColissionDetected { get; set; }
        public bool Go { get; set; }
        void Start()
        {
            Go = true;
            Ine = false;
            Physics.IgnoreLayerCollision(8, 9);
            NavAgent = GetComponent<NavMeshAgent>();
            BotRigidbody = GetComponent<Rigidbody>();
            Mesh = GetComponent<MeshCollider>();
        }
        private void FindNewTarget()
        {
            List<GameObject> Enemies = new List<GameObject>();
            Enemies.AddRange(GameObject.FindGameObjectsWithTag("Bot"));
            string thisName = this.gameObject.name;
            bool p = false;
            while (p == false)
            {
                int random = Random.Range(0, Enemies.Count);
                if (string.Compare(thisName, Enemies[random].name) != 0)
                {
                    p = true;
                    Target = Enemies[random];
                }
            }
        }
        private bool MTargets()
        {
            List<GameObject> Enemies = new List<GameObject>();
            Enemies.AddRange(GameObject.FindGameObjectsWithTag("Bot"));
            if (Enemies.Count > 1)
                return true;
            return false;
        }
        private void Update()
        {
            if (!MTargets() && GameObject.FindGameObjectWithTag("Player").activeSelf)
                Target = GameObject.FindGameObjectWithTag("Player");
            else if (!GameObject.FindGameObjectWithTag("Player").activeSelf)
                this.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            if (!Target.activeInHierarchy)
                FindNewTarget();
            if (this.transform.position.y <= Y)
                gameObject.SetActive(false);
        }
        private void FixedUpdate()
        {
            if (IsColissionDetected) 
                StartCoroutine(WakeUp());
            else 
                AssistFixedUpdate();

            IsColissionDetected = false;
        }
        private void AssistFixedUpdate()
        {

            if (NavAgent.isOnNavMesh)
                NavAgent.enabled = true;
            else
                NavAgent.enabled = false;

            if (NavAgent.enabled == true)
            {
                Mesh.enabled = false;
                Path = NavAgent.path;
                NavAgent.SetDestination(Target.transform.position);
                StartCoroutine(StartCoroutine());
            }
            else if (NavAgent.enabled == false && IsCarGrounded())
            {
                NavAgent.enabled = true;
                Mesh.enabled = false;
            }
            else if (NavAgent.enabled == false)
                Mesh.enabled = true;
            else if (!IsCarGrounded())
                Balance();
        }
        IEnumerator WakeUp()
        {
            yield return new WaitForSeconds(2f);
            Physics.IgnoreLayerCollision(8, 9);
            AssistFixedUpdate();
        }
        private bool IsRearWheelsGrounded()
        {
            IsCarGroundedRearWheels =
                      Physics.Raycast(Wheels[2].transform.position, transform.TransformDirection(Vector3.down), out _, 0.69f, GroundLayer) ||
                      Physics.Raycast(Wheels[3].transform.position, transform.TransformDirection(Vector3.down), out _, 0.69f, GroundLayer);
            return IsCarGroundedRearWheels;
        }
        private bool IsFrontWheelsGrounded()
        {
            IsCarGroundedFrontWheels =
                       Physics.Raycast(Wheels[4].transform.position, transform.TransformDirection(Vector3.down), out _, 0.69f, GroundLayer) ||
                       Physics.Raycast(Wheels[5].transform.position, transform.TransformDirection(Vector3.down), out _, 0.69f, GroundLayer);
            return IsCarGroundedFrontWheels;
        }
        private bool IsCarGrounded()
        {
            if (IsFrontWheelsGrounded() && IsRearWheelsGrounded())
                return true;
            return false;
        }
        private void Balance()
        {
            Mesh.enabled = true;
            if (!IsFrontWheelsGrounded() && IsRearWheelsGrounded())
            {
                NavAgent.enabled = false;
                BotRigidbody.MovePosition(MassFront.transform.position);
            }
            else if (IsFrontWheelsGrounded() && !IsRearWheelsGrounded())
            {
                NavAgent.enabled = false;
                BotRigidbody.MovePosition(MassRear.transform.position);
            }
        }
        private void ChangeRotation(float y)
        {
            Wheels[4].transform.localRotation = Quaternion.Slerp(
                                                           Wheels[4].transform.localRotation,
                                                           new Quaternion(Wheels[4].transform.localRotation.x,
                                                           Mathf.Clamp(y, -10, 10) * Time.deltaTime,
                                                           Wheels[4].transform.localRotation.z,
                                                           Wheels[4].transform.localRotation.w),
                                                           2 * Time.deltaTime * 0.5f);
            Wheels[5].transform.localRotation = Quaternion.Slerp(
                                                           Wheels[5].transform.localRotation,
                                                           new Quaternion(Wheels[5].transform.localRotation.x,
                                                           Mathf.Clamp(y, -10, 10) * Time.deltaTime,
                                                           Wheels[5].transform.localRotation.z,
                                                           Wheels[5].transform.localRotation.w),
                                                           2 * Time.deltaTime * 0.5f);
        }
        private void OnCollisionStay(Collision collision)
        {
            NavAgent.enabled = false;
            Mesh.enabled = true;
            Physics.IgnoreLayerCollision(8, 9, false);
            IsColissionDetected = true;
        }
        IEnumerator StartCoroutine()
        {
            //Wheels
            if (LastRotation == null)
                LastRotation = NavAgent.transform.localEulerAngles;

            NextRotation = NavAgent.transform.localEulerAngles;
            if (NextRotation != LastRotation)
            {
                if (NextRotation.y < LastRotation.y)
                    ChangeRotation(7);
                else if (NextRotation.y > LastRotation.y)
                    ChangeRotation(-7);
                else if (NextRotation.y == LastRotation.y)
                    ChangeRotation(0);
            }
            LastRotation = NavAgent.transform.localEulerAngles;

            for (int i = 0; i < Wheels.Count - 2; i++)
            {
                GameObject v = Wheels[i];
                v.transform.Rotate(NavAgent.acceleration, v.transform.rotation.y, v.transform.rotation.z);
            }

            //Curves
            if (Path.corners.Length / 2 > 1)
                Current = Path.corners[Path.corners.Length / 2];
            //else
            //    Current = Path.corners[0];

            float p = Current.x - NavAgent.transform.position.x;
            float a = Current.z - NavAgent.transform.position.z;
            if (p < HowMuchPositions && p > -HowMuchPositions && a < HowMuchPositions && a > -HowMuchPositions && Current != Path.corners[0])
            {
                NavAgent.speed = CurveSpeed;
                Ine = true;
            }
            else
            {
                if (Ine)
                {
                    yield return new WaitForSeconds(1);
                    if (NavAgent.speed < MaxSpeed)
                        NavAgent.speed += 2;
                    else
                        NavAgent.speed = MaxSpeed - 1;
                }
                else
                {
                    if (NavAgent.speed < MaxSpeed)
                        NavAgent.speed += 4;
                    else
                        NavAgent.speed = MaxSpeed - 1;
                }
            }
        }
    }
}
