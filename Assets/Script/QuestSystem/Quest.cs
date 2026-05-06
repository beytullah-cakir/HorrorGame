using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public enum QuestState
    {
        Locked,
        Active,
        Completed
    }

    [System.Serializable]
    public class SubTask
    {
        public string description;
        public bool isCompleted;
        
        [Header("Counter Settings (Optional)")]
        public int targetCount = 0; // If > 0, this task uses a counter
        public int currentCount = 0;

        public SubTask(string description)
        {
            this.description = description;
            this.isCompleted = false;
        }

        public bool CheckCompletion()
        {
            if (targetCount > 0)
            {
                return currentCount >= targetCount;
            }
            return isCompleted;
        }
    }

    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest")]
    public class Quest : ScriptableObject
    {
        
        public string title;
        public QuestState state;
        public bool autoActivateNext = true; 
        public List<SubTask> subTasks = new List<SubTask>();

        public bool AreAllSubTasksCompleted()
        {
            if (subTasks == null || subTasks.Count == 0) return true;
            
            foreach (var subTask in subTasks)
            {
                if (!subTask.CheckCompletion()) return false;
            }
            return true;
        }
    }
}
