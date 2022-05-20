using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Osiris.Controllers.PlaneController
{
    public class PlaneController : MonoBehaviour
    {
        [SerializeField] [ReorderableList] List<Transform> Points;
        [SerializeField] private GameObject AirCraft;
        [SerializeField] private GameObject Parachute;
        [SerializeField] private GameObject Player;
        [SerializeField] private int Speed;
        private float Distance;
        private Transform NextWayPoint;
        private void Start()
        {
            Instantiate(AirCraft, Points[Random.Range(1, 4)]);
        }
        private void Update()
        {
            Distance = AirCraft.transform.position.y - Player.transform.position.y;
            Vector3 NewPosition = new Vector3(Player.transform.position.x, Distance + Player.transform.position.y, Player.transform.position.z);
            AirCraft.transform.LookAt(NewPosition);
            this.transform.localPosition = Vector3.MoveTowards(this.transform.position, Player.transform.position, Speed * Time.deltaTime);
            if (Vector3.Distance(this.transform.position, NewPosition) <= 1)
                NextWayPoint = Points[2];
            Debug.Log(this.transform.localPosition);
            Debug.Log(this.transform.position);
        }
    }
}
