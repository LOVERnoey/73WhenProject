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
        }
        public List<WorkerPrefabEntry> workerPrefabs; // Assign in inspector
        private Dictionary<string, GameObject> spawnedWorkers = new Dictionary<string, GameObject>();

        private GameObject GetPrefabForWorker(string workerName)
        {
            foreach (var entry in workerPrefabs)
            {
                if (entry.workerName == workerName)
                    return entry.prefab;
            }
            return null;
        }

        public void SpawnWorkers(List<string> namesToSpawn)
        {
            // Clear previous workers
            foreach (var worker in spawnedWorkers.Values)
            {
                Destroy(worker);
            }
            spawnedWorkers.Clear();

            // Spawn new workers
            foreach (var workerName in namesToSpawn)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                GameObject prefab = GetPrefabForWorker(workerName);
                if (prefab != null)
                {
                    GameObject worker = Instantiate(prefab, spawnPos, Quaternion.identity);
                    worker.name = workerName;
                    // Optionally set NPC display name here
                    spawnedWorkers.Add(workerName, worker);
                }
                else
                {
                    Debug.LogWarning($"No prefab found for worker: {workerName}");
                }
            }
        }

        public int CheckWorkerAttendance(List<string> checklist)
        {
            int present = 0;
            foreach (var name in checklist)
            {
                if (spawnedWorkers.ContainsKey(name))
                    present++;
            }
            return present;
        }
    }
}
