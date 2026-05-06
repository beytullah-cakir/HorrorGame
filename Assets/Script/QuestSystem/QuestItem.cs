using UnityEngine;

namespace QuestSystem
{
    public class QuestItem : InteractableBase
    {
        [Header("Collection Settings")]
        [SerializeField] protected Quest requiredQuest;
        [SerializeField] protected bool completeSubTask = true;
        [SerializeField] protected int subTaskIndex = 0;
        [SerializeField] protected bool interactToCollect = true;
        [SerializeField] protected bool requiresBox = true;
        [SerializeField] protected bool destroyOnCollect = true;


        public override void Interact()
        {
            // 1. Görev sırası kontrolü
            if (!IsQuestActive())
            {
                return;
            }

            
            if (requiresBox)
            {
                if (PlayerCarryController.Instance == null || !PlayerCarryController.Instance.HasBox())
                {
                    return;
                }
            }

            if (interactToCollect)
            {
                Collect();
            }
        }

        public override bool CanInteract()
        {
            return IsQuestActive();
        }

        protected bool IsQuestActive()
        {
            Quest activeQuest = QuestManager.Instance.GetActiveQuest();
            return requiredQuest == null || activeQuest == requiredQuest;
        }

        public int GetSubTaskIndex()
        {
            return subTaskIndex;
        }

        public virtual void Collect()
        {
            if (completeSubTask)
            {
                QuestManager.Instance.CompleteSubTask(subTaskIndex);
            }
            else
            {
                QuestManager.Instance.CompleteCurrentQuest();
            }

            
            if (PlayerCarryController.Instance != null && requiresBox)
            {
                PlayerCarryController.Instance.ShowBox(true);
            }
            if (destroyOnCollect)
            {
                Destroy(gameObject);
            }
        }
    }
}
