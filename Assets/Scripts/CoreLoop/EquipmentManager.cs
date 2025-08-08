using UnityEngine;
using System.Collections.Generic;

namespace CoreLoop
{
    public class EquipmentManager : MonoBehaviour
    {
        // Handles spawning and checking equipment
        public GameObject equipmentPrefab; // Assign equipment prefab in inspector
        private List<GameObject> spawnedEquipment = new List<GameObject>();

        public void SpawnEquipment(List<string> equipmentList)
        {
            // Clear previous equipment
            foreach (var eq in spawnedEquipment)
            {
                Destroy(eq);
            }
            spawnedEquipment.Clear();

            // Spawn new equipment
            foreach (var name in equipmentList)
            {
                Vector3 spawnPos = new Vector3(Random.Range(5, 15), 0, Random.Range(-5, 5));
                GameObject eq = Instantiate(equipmentPrefab, spawnPos, Quaternion.identity);
                eq.name = name;
                // Optionally set equipment display name here
                spawnedEquipment.Add(eq);
            }
        }

        public int CheckEquipment(List<string> checklist)
        {
            int present = 0;
            foreach (var name in checklist)
            {
                if (spawnedEquipment.Exists(e => e.name == name))
                    present++;
            }
            return present;
        }
    }
}
