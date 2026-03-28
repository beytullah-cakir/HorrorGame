using UnityEngine;

namespace QuestSystem
{
    public class QuestTrigger : InteractableBase
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
        public bool isInteractableTrigger = false;
        private bool hasTriggered = false;
        

        private void OnTriggerEnter(Collider other)
        {
            if (isInteractableTrigger) return; // Wait for interaction
            if (hasTriggered && triggerOnce) return;
            if (!other.CompareTag("Player")) return;

            ExecuteTrigger();
        }

        public override void Interact()
        {
            if (isInteractableTrigger)
            {
                ExecuteTrigger();
            }
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
        }
    }
}
