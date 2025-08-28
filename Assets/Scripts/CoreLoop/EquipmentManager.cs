using UnityEngine;
using System.Collections.Generic;

namespace CoreLoop
{
    public class EquipmentManager : MonoBehaviour
    {
        // Handles spawning and checking equipment
        [System.Serializable]
        public class EquipmentPrefabEntry
        {
            public string equipmentName;
            public GameObject prefab;
        }
        public List<EquipmentPrefabEntry> equipmentPrefabs; // Assign in inspector
        private Dictionary<string, GameObject> spawnedEquipment = new Dictionary<string, GameObject>();

        private GameObject GetPrefabForEquipment(string equipmentName)
        {
            foreach (var entry in equipmentPrefabs)
            {
                if (entry.equipmentName == equipmentName)
                    return entry.prefab;
            }
            return null;
        }

        public void SpawnEquipment(List<string> equipmentList)
        {
            // Clear previous equipment
            foreach (var eq in spawnedEquipment.Values)
            {
                Destroy(eq);
            }
            spawnedEquipment.Clear();

            // Spawn new equipment
            foreach (var name in equipmentList)
            {
                Vector3 spawnPos = new Vector3(Random.Range(5, 15), 0, Random.Range(-5, 5));
                GameObject prefab = GetPrefabForEquipment(name);
                if (prefab != null)
                {
                    GameObject eq = Instantiate(prefab, spawnPos, Quaternion.identity);
                    eq.name = name;
                    // Optionally set equipment display name here
                    spawnedEquipment.Add(name, eq);
                }
                else
                {
                    Debug.LogWarning($"No prefab found for equipment: {name}");
                }
            }
        }

        public int CheckEquipment(List<string> checklist)
        {
            int present = 0;
            foreach (var name in checklist)
            {
                if (spawnedEquipment.ContainsKey(name))
                    present++;
            }
            return present;
        }
    }
}
