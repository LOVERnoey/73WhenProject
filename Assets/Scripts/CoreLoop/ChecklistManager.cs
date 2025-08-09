using UnityEngine;
using System.Collections.Generic;

namespace CoreLoop
{
    public class ChecklistManager : MonoBehaviour
    {
        // Handles daily checklist UI and randomization

        // Checklist data for the day
        [Header("Daily Checklist Data")]
        [HideInInspector] private List<string> workersToCheck = new List<string>();
        [HideInInspector] private List<string> equipmentToCheck = new List<string>();
        [HideInInspector] private List<string> constructionSitesToCheck = new List<string>();
        [HideInInspector] private List<string> obstacles = new List<string>();
        public int workersRequired;
        public int equipmentRequired;
        public int constructionRequired;

        // Example: possible names/items (should be loaded from data in real use)
        [Header("Possible Names and Items")]
        [SerializeField] private List<string> allWorkerNames = new List<string> { "Somchai", "Nattapong", "Suda", "Kanya", "Prasit", "Wirote", "Malee" };
        [SerializeField] private List<string> allEquipment = new List<string> { "Hammer", "Drill", "Saw", "Helmet", "Gloves", "Ladder" };
        [SerializeField] private List<string> allConstructionSites = new List<string> { "BuildingA", "BuildingB", "BuildingC" };
        [SerializeField] private List<string> allObstacles = new List<string> { "Rain", "PowerOutage", "LateDelivery", "BrokenTool" };

        // References to managers (assign in inspector or via script)
        public WorkerManager workerManager;
        public EquipmentManager equipmentManager;
        public ConstructionQualityManager constructionQualityManager;
        public ChecklistUIManager checklistUIManager; // Assign in Inspector
        public int currentDay = 1;

        void Start()
        {
            StartDay();
            ShowChecklistPanel();
            Debug.Log("Starting Day " + currentDay);
        }
        
        public void GenerateChecklist(int day)
        {
            // Increase difficulty by day
            workersRequired = Mathf.Min(2 + day, allWorkerNames.Count);
            equipmentRequired = Mathf.Min(2 + day, allEquipment.Count);
            constructionRequired = Mathf.Min(1 + day / 3, allConstructionSites.Count);

            workersToCheck = GetRandomList(allWorkerNames, workersRequired);
            equipmentToCheck = GetRandomList(allEquipment, equipmentRequired);
            constructionSitesToCheck = GetRandomList(allConstructionSites, constructionRequired);
            obstacles = GetRandomList(allObstacles, Mathf.Min(1 + day / 2, allObstacles.Count));
        }

        private List<string> GetRandomList(List<string> source, int count)
        {
            List<string> copy = new List<string>(source);
            List<string> result = new List<string>();
            for (int i = 0; i < count && copy.Count > 0; i++)
            {
                int idx = Random.Range(0, copy.Count);
                result.Add(copy[idx]);
                copy.RemoveAt(idx);
            }
            return result;
        }

        public void ShowChecklistPanel()
        {
            // Show all possible items, not just spawned ones
            List<string> checklistItems = new List<string>();
            checklistItems.AddRange(allWorkerNames);
            checklistItems.AddRange(allEquipment);
            checklistItems.AddRange(allConstructionSites);
            checklistUIManager.ShowChecklist($"Day {currentDay} Checklist", checklistItems, (checkedItems) => {
                Debug.Log("Checked: " + string.Join(", ", checkedItems));
            });
        }

        // Call this to start a new day
        public void StartDay()
        {
            GenerateChecklist(currentDay);
            workerManager.SpawnWorkers(workersToCheck);
            equipmentManager.SpawnEquipment(equipmentToCheck);
            constructionQualityManager.SpawnConstructionSites(constructionSitesToCheck);
            ShowChecklistPanel();
        }

        // Call this to submit checklist and save results
        public void SubmitChecklist()
        {
            List<string> checkedItems = checklistUIManager.GetCheckedItems();
            int score = 0;

            // Combine all spawned items
            List<string> allSpawned = new List<string>();
            allSpawned.AddRange(workersToCheck);
            allSpawned.AddRange(equipmentToCheck);
            allSpawned.AddRange(constructionSitesToCheck);

            // +1 for each correctly checked spawned item, -1 for each missed spawned item
            foreach (var item in allSpawned)
            {
                if (checkedItems.Contains(item))
                    score += 1;
                else
                    score -= 1;
            }

            // -1 for each checked item that was NOT spawned
            foreach (var checkedItem in checkedItems)
            {
                if (!allSpawned.Contains(checkedItem))
                    score -= 1;
            }

            Debug.Log($"Checklist Score: {score}");
            // For demo, assume all obstacles missed
            List<string> missedObstacles = new List<string>(obstacles);

            var result = new DailyChecklistResult(currentDay)
            {
                workersChecked = workerManager.CheckWorkerAttendance(workersToCheck),
                workersTotal = workersToCheck.Count,
                equipmentChecked = equipmentManager.CheckEquipment(equipmentToCheck),
                equipmentTotal = equipmentToCheck.Count,
                constructionQualityScore = constructionQualityManager.CheckConstructionQuality(constructionSitesToCheck),
                missedObstacles = missedObstacles
            };
            // Save to GameData (pseudo, replace with your save system)
            // GameData.Instance.dailyChecklistResults.Add(result);
            Debug.Log($"Day {currentDay} Results: Workers {result.workersChecked}/{result.workersTotal}, Equipment {result.equipmentChecked}/{result.equipmentTotal}, Quality {result.constructionQualityScore}");
            currentDay++;
            
        }
    }
}
