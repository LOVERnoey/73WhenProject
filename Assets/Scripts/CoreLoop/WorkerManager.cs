using UnityEngine;
using System.Collections.Generic;

namespace CoreLoop
{
    public class WorkerManager : MonoBehaviour
    {
        // Handles spawning and checking workers
        [System.Serializable]
        public class WorkerPrefabEntry
        {
            public string workerName;
            public GameObject prefab;
            public float x; // set in Inspector
            public float y = 0f; // vertical offset
            public float z; // set in Inspector
        }
        public List<WorkerPrefabEntry> workerPrefabs; // Assign in inspector
        private readonly Dictionary<string, GameObject> spawnedWorkers = new Dictionary<string, GameObject>();

        private WorkerPrefabEntry FindEntry(string workerName) => workerPrefabs.Find(e => e.workerName == workerName);

        public void SpawnWorkers(List<string> namesToSpawn)
        {
            // Clear previous so scene contains exactly the checklist workers
            foreach (var w in spawnedWorkers.Values)
            {
                if (w) Destroy(w);
            }
            spawnedWorkers.Clear();
            foreach (var name in namesToSpawn)
            {
                SpawnSingle(name, replaceIfExists: true);
            }
        }

        public void EnsureWorkersSpawned(List<string> names)
        {
            foreach (var name in names)
            {
                if (!spawnedWorkers.ContainsKey(name))
                    SpawnSingle(name, replaceIfExists: false);
            }
        }

        private void SpawnSingle(string name, bool replaceIfExists)
        {
            if (spawnedWorkers.ContainsKey(name))
            {
                if (!replaceIfExists) return;
                var existing = spawnedWorkers[name];
                if (existing) Destroy(existing);
                spawnedWorkers.Remove(name);
            }
            var entry = FindEntry(name);
            if (entry == null || entry.prefab == null)
            {
                Debug.LogWarning($"[WorkerManager] Missing entry or prefab for '{name}'");
                return;
            }
            Vector3 pos = new Vector3(entry.x, entry.y, entry.z);
            var worker = Instantiate(entry.prefab, pos, Quaternion.identity);
            worker.name = name;
            spawnedWorkers[name] = worker;
            Debug.Log($"[WorkerManager] Spawned '{name}' at {pos}");
        }

        public int CheckWorkerAttendance(List<string> checklist)
        {
            EnsureWorkersSpawned(checklist);
            int present = 0;
            foreach (var name in checklist)
            {
                if (spawnedWorkers.ContainsKey(name) && spawnedWorkers[name] != null)
                    present++;
            }
            return present;
        }
    }
}
