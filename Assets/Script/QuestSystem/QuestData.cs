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

        public SubTask(string description)
        {
            this.description = description;
            this.isCompleted = false;
        }
    }

    [System.Serializable]
    public class Quest
    {
        public string title;
        public QuestState state;
        public List<SubTask> subTasks = new List<SubTask>();

        public bool AreAllSubTasksCompleted()
        {
            if (subTasks == null || subTasks.Count == 0) return true;
            
            foreach (var subTask in subTasks)
            {
                if (!subTask.isCompleted) return false;
            }
            return true;
        }
    }
}
