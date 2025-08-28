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
        }
        public List<ConstructionPrefabEntry> constructionPrefabs; // Assign in inspector
        private Dictionary<string, GameObject> spawnedSites = new Dictionary<string, GameObject>();
        private Dictionary<string, int> siteQuality = new Dictionary<string, int>();

        // Handles checking construction quality
        public void InspectConstruction() { /* TODO: Implement inspection logic */ }

        private GameObject GetPrefabForSite(string siteName)
        {
            foreach (var entry in constructionPrefabs)
            {
                if (entry.siteName == siteName)
                    return entry.prefab;
            }
            return null;
        }

        public void SpawnConstructionSites(List<string> siteList)
        {
            // Clear previous sites
            foreach (var site in spawnedSites.Values)
            {
                Destroy(site);
            }
            spawnedSites.Clear();
            siteQuality.Clear();

            // Spawn new sites
            foreach (var name in siteList)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-10, -5), 0, Random.Range(-5, 5));
                GameObject prefab = GetPrefabForSite(name);
                if (prefab != null)
                {
                    GameObject site = Instantiate(prefab, spawnPos, Quaternion.identity);
                    site.name = name;
                    spawnedSites.Add(name, site);
                    // Random quality score for demo (0-100)
                    siteQuality[name] = Random.Range(60, 100);
                }
                else
                {
                    Debug.LogWarning($"No prefab found for construction site: {name}");
                }
            }
        }

        public int CheckConstructionQuality(List<string> checklist)
        {
            int totalScore = 0;
            foreach (var name in checklist)
            {
                if (siteQuality.ContainsKey(name))
                    totalScore += siteQuality[name];
            }
            return checklist.Count > 0 ? totalScore / checklist.Count : 0;
        }
    }
}
