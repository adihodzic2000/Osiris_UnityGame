using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Osiris.Controllers.CarController;
using UnityEngine;
using UnityEngine.AI;

namespace Osiris.Generators
{
    public class Spawn : MonoBehaviour
    {
        [SerializeField] [ReorderableList] List<Transform> SpawnPositions;
        [SerializeField] [Range(1, 8)] private int NumberOfBots;
        [SerializeField] private GameObject Prefab;
        [SerializeField] private float Y;

        private void Start()
        {
            for (int i = 0; i < NumberOfBots; i++)
            {
                Prefab.gameObject.name = $"Bot" + i;
                Prefab.GetComponent<Rigidbody>().mass = Random.Range(10, 20);
                Instantiate(Prefab, SpawnPositions[i % SpawnPositions.Count]);
            }
            for (int i = 0; i < NumberOfBots; i++)
            {
                int number = Random.Range(0, NumberOfBots);
                if (number != i)
                    GameObject.Find($"Bot" + i + $"(Clone)").GetComponent<BotController>().Target = GameObject.Find($"Bot" + number + $"(Clone)");
                else
                    i--;
            }
        }
    }
}
