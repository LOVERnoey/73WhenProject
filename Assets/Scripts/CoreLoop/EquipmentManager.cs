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
            public float x; // set in Inspector
            public float y = 0f; // vertical offset
            public float z; // set in Inspector
        }
        public List<EquipmentPrefabEntry> equipmentPrefabs; // Assign in inspector
        private readonly Dictionary<string, GameObject> spawnedEquipment = new Dictionary<string, GameObject>();

        private EquipmentPrefabEntry FindEntry(string equipmentName) => equipmentPrefabs.Find(e => e.equipmentName == equipmentName);

        public void SpawnEquipment(List<string> equipmentList)
        {
            // Clear previous equipment so we always show ONLY the checklist contents
            foreach (var eq in spawnedEquipment.Values)
            {
                if (eq) Destroy(eq);
            }
            spawnedEquipment.Clear();

            foreach (var name in equipmentList)
            {
                SpawnSingle(name, replaceIfExists: true);
            }
        }

        public void EnsureEquipmentSpawned(List<string> equipmentList)
        {
            foreach (var name in equipmentList)
            {
                if (!spawnedEquipment.ContainsKey(name))
                    SpawnSingle(name, replaceIfExists: false);
            }
        }

        private void SpawnSingle(string name, bool replaceIfExists)
        {
            if (spawnedEquipment.ContainsKey(name))
            {
                if (!replaceIfExists) return;
                var existing = spawnedEquipment[name];
                if (existing) Destroy(existing);
                spawnedEquipment.Remove(name);
            }

            var entry = FindEntry(name);
            if (entry == null || entry.prefab == null)
            {
                Debug.LogWarning($"[EquipmentManager] Missing entry or prefab for '{name}'");
                return;
            }
            Vector3 spawnPos = new Vector3(entry.x, entry.y, entry.z);
            var eq = Instantiate(entry.prefab, spawnPos, Quaternion.identity);
            eq.name = name;
            spawnedEquipment[name] = eq;
            Debug.Log($"[EquipmentManager] Spawned '{name}' at {spawnPos}");
        }

        public int CheckEquipment(List<string> checklist)
        {
            // Guarantee everything in checklist is present (no random omission)
            EnsureEquipmentSpawned(checklist);
            int present = 0;
            foreach (var name in checklist)
            {
                if (spawnedEquipment.ContainsKey(name) && spawnedEquipment[name] != null)
                    present++;
            }
            return present;
        }
    }
}
