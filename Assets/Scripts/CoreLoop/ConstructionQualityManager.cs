using System.Collections.Generic;
using UnityEngine;

namespace CoreLoop
{
    public class ConstructionQualityManager : MonoBehaviour
    {
        [System.Serializable]
        public class ConstructionPrefabEntry
        {
            public string siteName;
            public GameObject prefab;
            public float x; // set in Inspector
            public float y = 0f; // vertical offset
            public float z; // set in Inspector
            public int defaultQuality = 80; // fallback quality
        }
        public List<ConstructionPrefabEntry> constructionPrefabs; // Assign in inspector
        private readonly Dictionary<string, GameObject> spawnedSites = new Dictionary<string, GameObject>();
        private readonly Dictionary<string, int> siteQuality = new Dictionary<string, int>();

        private ConstructionPrefabEntry FindEntry(string siteName) => constructionPrefabs.Find(e => e.siteName == siteName);

        public void SpawnConstructionSites(List<string> siteList)
        {
            foreach (var s in spawnedSites.Values)
            {
                if (s) Destroy(s);
            }
            spawnedSites.Clear();
            siteQuality.Clear();
            foreach (var name in siteList)
            {
                SpawnSingle(name, replaceIfExists: true, setQualityIfMissing: true);
            }
        }

        public void EnsureSitesSpawned(List<string> siteList)
        {
            foreach (var name in siteList)
            {
                if (!spawnedSites.ContainsKey(name))
                    SpawnSingle(name, replaceIfExists: false, setQualityIfMissing: true);
            }
        }

        private void SpawnSingle(string name, bool replaceIfExists, bool setQualityIfMissing)
        {
            if (spawnedSites.ContainsKey(name))
            {
                if (!replaceIfExists) return;
                var existing = spawnedSites[name];
                if (existing) Destroy(existing);
                spawnedSites.Remove(name);
            }
            var entry = FindEntry(name);
            if (entry == null || entry.prefab == null)
            {
                Debug.LogWarning($"[ConstructionQualityManager] Missing entry or prefab for '{name}'");
                return;
            }
            Vector3 pos = new Vector3(entry.x, entry.y, entry.z);
            var site = Instantiate(entry.prefab, pos, Quaternion.identity);
            site.name = name;
            spawnedSites[name] = site;
            if (setQualityIfMissing && !siteQuality.ContainsKey(name))
            {
                siteQuality[name] = entry.defaultQuality;
            }
            Debug.Log($"[ConstructionQualityManager] Spawned site '{name}' at {pos} quality={siteQuality[name]}");
        }

        public int CheckConstructionQuality(List<string> checklist)
        {
            EnsureSitesSpawned(checklist);
            if (checklist.Count == 0) return 0;
            int total = 0;
            int counted = 0;
            foreach (var name in checklist)
            {
                if (siteQuality.ContainsKey(name))
                {
                    total += siteQuality[name];
                    counted++;
                }
            }
            return counted > 0 ? total / counted : 0;
        }
    }
}
