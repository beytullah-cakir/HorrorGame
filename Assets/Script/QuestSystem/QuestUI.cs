using UnityEngine;
using TMPro;
using System.Text;

namespace QuestSystem
{
    public class QuestUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text questTitleText;
        
        [SerializeField] private TMP_Text subTasksText;

        [Header("Styling")]
        [SerializeField] private string completedAlpha = "88"; // Hex alpha for fading
        [SerializeField] private string pendingColor = "white";

        private void OnEnable()
        {
            QuestManager.OnQuestActivated += UpdateUI;
            QuestManager.OnQuestCompleted += HandleQuestCompleted;
            QuestManager.OnSubTaskCompleted += HandleSubTaskCompleted;
        }

        private void OnDisable()
        {
            QuestManager.OnQuestActivated -= UpdateUI;
            QuestManager.OnQuestCompleted -= HandleQuestCompleted;
            QuestManager.OnSubTaskCompleted -= HandleSubTaskCompleted;
        }

        private void UpdateUI(Quest activeQuest)
        {
            if (activeQuest == null)
            {
                ClearUI();
                return;
            }

            if (questTitleText != null) 
                questTitleText.text = activeQuest.title;

            RefreshSubTasks(activeQuest);
        }

        private void RefreshSubTasks(Quest activeQuest)
        {
            if (subTasksText == null) return;

            if (activeQuest.subTasks == null || activeQuest.subTasks.Count == 0)
            {
                subTasksText.text = "";
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var subTask in activeQuest.subTasks)
            {
                
                string content = subTask.isCompleted ? $"<s>{subTask.description}</s>" : subTask.description;
                
                if (subTask.isCompleted)
                {
                    sb.AppendLine($"<alpha=#{completedAlpha}>{content}");
                }
                else
                {
                    sb.AppendLine($"<color={pendingColor}>{content}</color>");
                }
            }

            subTasksText.text = sb.ToString();
        }

        private void HandleQuestCompleted(Quest quest)
        {
            if (questTitleText != null)
                questTitleText.text = $"<alpha=#{completedAlpha}><s>{quest.title}</s>";
            
            Debug.Log($"[QuestUI] Quest {quest.title} visual marked as completed (faded + strikethrough).");
        }

        private void HandleSubTaskCompleted(Quest quest, int index)
        {
            RefreshSubTasks(quest);
        }

        private void ClearUI()
        {
            if (questTitleText != null) questTitleText.text = "No Active Quest";
            if (subTasksText != null) subTasksText.text = "";
        }
    }
}
