using UnityEngine;
using QuestSystem;

namespace QuestSystem
{
    public class QuestBoxController : MonoBehaviour
    {
        [Header("Quest Settings")]
        [SerializeField] private Quest targetQuestObject;
        [Tooltip("If true, it will react to any quest completion. Use targetQuestObject if you want specific quests.")]
        [SerializeField] private bool reactToAnyQuest = false;

        [Header("Animation Settings")]
        [SerializeField] private Animator boxAnimator;
        [SerializeField] private string closeTriggerName = "Close";

        private void OnEnable()
        {
            QuestManager.OnQuestCompleted += HandleQuestCompleted;
        }

        private void OnDisable()
        {
            QuestManager.OnQuestCompleted -= HandleQuestCompleted;
        }

        private void HandleQuestCompleted(Quest quest)
        {
            if (reactToAnyQuest || quest == targetQuestObject)
            {
                CloseBox();
            }
        }

        private void CloseBox()
        {
            if (boxAnimator != null)
            {
                boxAnimator.SetTrigger(closeTriggerName);
                Debug.Log($"[QuestBoxController] {gameObject.name} closing box because quest '{targetQuestObject?.title}' completed.");
            }
            else
            {
                Debug.LogWarning($"[QuestBoxController] {gameObject.name} Animator is not assigned!");
            }
        }
    }
}
