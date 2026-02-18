using UnityEngine;

namespace QuestSystem
{
    public class QuestTrigger : MonoBehaviour
    {
        public enum TriggerType
        {
            ActivateNext,
            CompleteSubTask,
            CompleteCurrent
        }

        public TriggerType type;
        public int subTaskIndex;
        public bool triggerOnce = true;
        private bool hasTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (hasTriggered && triggerOnce) return;
            if (!other.CompareTag("Player")) return;

            ExecuteTrigger();
        }

        public void ExecuteTrigger()
        {
            if (hasTriggered && triggerOnce) return;

            switch (type)
            {
                case TriggerType.ActivateNext:
                    QuestManager.Instance.ActivateNextQuest();
                    break;
                case TriggerType.CompleteSubTask:
                    QuestManager.Instance.CompleteSubTask(subTaskIndex);
                    break;
                case TriggerType.CompleteCurrent:
                    QuestManager.Instance.CompleteCurrentQuest();
                    break;
            }

            hasTriggered = true;
            Debug.Log($"[QuestTrigger] {type} triggered!");
        }
    }
}
