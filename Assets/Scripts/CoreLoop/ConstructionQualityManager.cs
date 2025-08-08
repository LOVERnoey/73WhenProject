using System.Collections.Generic;
using UnityEngine;

namespace CoreLoop
{
    public class ConstructionQualityManager : MonoBehaviour
    {
        public GameObject constructionPrefab; // Assign construction site prefab in inspector
        private List<GameObject> spawnedSites = new List<GameObject>();
        private Dictionary<string, int> siteQuality = new Dictionary<string, int>();

        // Handles checking construction quality
        public void InspectConstruction() { /* TODO: Implement inspection logic */ }

        public void SpawnConstructionSites(List<string> siteList)
        {
            // Clear previous sites
            foreach (var site in spawnedSites)
            {
                Destroy(site);
            }
            spawnedSites.Clear();
            siteQuality.Clear();

            // Spawn new sites
            foreach (var name in siteList)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-10, -5), 0, Random.Range(-5, 5));
                GameObject site = Instantiate(constructionPrefab, spawnPos, Quaternion.identity);
                site.name = name;
                spawnedSites.Add(site);
                // Random quality score for demo (0-100)
                siteQuality[name] = Random.Range(60, 100);
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
