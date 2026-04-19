using UnityEngine;

namespace QuestSystem
{
    public class QuestItem : InteractableBase
    {
        [Header("Collection Settings")]
        [SerializeField] private Quest requiredQuest;
        [SerializeField] private bool completeSubTask = true;
        [SerializeField] private int subTaskIndex = 0;
        [SerializeField] private bool interactToCollect = true;
        [SerializeField] private bool requiresBox = true;
        
        [Header("Effects")]
        [SerializeField] private GameObject collectEffect;
        [SerializeField] private AudioClip collectSound;






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

        private bool IsQuestActive()
        {
            Quest activeQuest = QuestManager.Instance.GetActiveQuest();
            return requiredQuest == null || activeQuest == requiredQuest;
        }

        public void Collect()
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

            if (collectEffect != null)
            {
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            }

            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }
            
            Destroy(gameObject);
        }
    }
}
