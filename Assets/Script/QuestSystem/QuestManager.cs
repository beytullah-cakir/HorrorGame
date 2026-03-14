using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }

        [Header("Quest Settings")]
        [SerializeField] private List<Quest> quests = new List<Quest>();
        [SerializeField] private int currentQuestIndex = 0;

        // Events
        public static event Action<Quest> OnQuestActivated;
        public static event Action<Quest> OnQuestCompleted;
        public static event Action<Quest, int> OnSubTaskCompleted;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            InitializeQuests();
        }

        private void InitializeQuests()
        {
            if (quests == null || quests.Count == 0)
            {
                Debug.LogWarning("[QuestManager] No quests found in the list!");
                return;
            }

            // Set all quests to Locked initially
            for (int i = 0; i < quests.Count; i++)
            {
                quests[i].state = QuestState.Locked;
            }

            // Automatically activate the first quest (index 0)
            ActivateQuest(0);
            
            Debug.Log("[QuestManager] System initialized. First quest activated automatically.");
        }

        public void ActivateNextQuest()
        {
            // If nothing is active and we are at index 0, check if we should start the first quest
            if (currentQuestIndex == 0 && quests[0].state == QuestState.Locked)
            {
                ActivateQuest(0);
                return;
            }

            int nextIndex = currentQuestIndex + 1;

            if (nextIndex >= quests.Count)
            {
                Debug.Log("[QuestManager] All quests are already completed or no more quests available.");
                return;
            }

            // A quest can only be activated if the current one is completed
            if (quests[currentQuestIndex].state != QuestState.Completed)
            {
                Debug.LogWarning("[QuestManager] Complete the current quest before activating the next one!");
                return;
            }

            ActivateQuest(nextIndex);
        }

        public void ActivateQuest(int index)
        {
            if (index < 0 || index >= quests.Count) return;

            currentQuestIndex = index;
            quests[currentQuestIndex].state = QuestState.Active;
            
            Debug.Log($"[QuestManager] Activated Quest: {quests[currentQuestIndex].title}");
            OnQuestActivated?.Invoke(quests[currentQuestIndex]);
        }

        public void CompleteSubTask(int subTaskIndex)
        {
            Quest activeQuest = GetActiveQuest();
            if (activeQuest == null) return;

            if (subTaskIndex < 0 || subTaskIndex >= activeQuest.subTasks.Count)
            {
                Debug.LogError($"[QuestManager] SubTask index {subTaskIndex} out of bounds for quest {activeQuest.title}");
                return;
            }

            if (activeQuest.subTasks[subTaskIndex].isCompleted) return;

            activeQuest.subTasks[subTaskIndex].isCompleted = true;
            Debug.Log($"[QuestManager] SubTask Completed: {activeQuest.subTasks[subTaskIndex].description}");
            
            OnSubTaskCompleted?.Invoke(activeQuest, subTaskIndex);

            // Check if all subtasks are done
            if (activeQuest.AreAllSubTasksCompleted())
            {
                CompleteCurrentQuest();
            }
        }

        public void CompleteCurrentQuest()
        {
            Quest activeQuest = GetActiveQuest();
            if (activeQuest == null || activeQuest.state == QuestState.Completed) return;

            // Optional check for subtasks if they weren't manually completed via CompleteSubTask
            if (!activeQuest.AreAllSubTasksCompleted())
            {
                Debug.LogWarning("[QuestManager] Cannot complete quest! Subtasks are still pending.");
                return;
            }

            activeQuest.state = QuestState.Completed;
            Debug.Log($"[QuestManager] Quest Completed: {activeQuest.title}");
            
            OnQuestCompleted?.Invoke(activeQuest);

            // Transition based on quest settings
            if (activeQuest.autoActivateNext)
            {
                ActivateNextQuest();
            }
            else
            {
                Debug.Log($"[QuestManager] Quest '{activeQuest.title}' completed, but next quest requires trigger.");
            }
        }

        public Quest GetActiveQuest()
        {
            if (currentQuestIndex < 0 || currentQuestIndex >= quests.Count) return null;
            
            Quest q = quests[currentQuestIndex];
            return q.state == QuestState.Active ? q : null;
        }

        #region Save/Load System Ready
        
        public int GetCurrentQuestIndex() => currentQuestIndex;
        
        public List<Quest> GetAllQuests() => quests;

        public void LoadQuestData(int index, List<QuestState> states, List<List<bool>> subTaskStates)
        {
            currentQuestIndex = index;
            for (int i = 0; i < quests.Count && i < states.Count; i++)
            {
                quests[i].state = states[i];
                // Load subtask states if provided logic here...
            }
        }

        #endregion
    }
}
