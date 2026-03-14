using UnityEngine;

namespace QuestSystem
{
    public class QuestItem : InteractableBase
    {
        [Header("Collection Settings")]
        [SerializeField] private Quest requiredQuest; // This item can only be collected if this quest is active
        [SerializeField] private bool completeSubTask = true;
        [SerializeField] private int subTaskIndex = 0;
        [SerializeField] private bool interactToCollect = true;
        
        [Header("Effects")]
        [SerializeField] private GameObject collectEffect;
        [SerializeField] private AudioClip collectSound;

        protected override void Awake()
        {
            base.Awake();
        }


        public override void OnHoverEnter()
        {
            if (!IsQuestActive()) return;
            base.OnHoverEnter();
        }

        public override void OnHoverExit()
        {
            if (!IsQuestActive()) return;
            base.OnHoverExit();
        }

        public override void Interact()
        {
            // 1. Görev sırası kontrolü
            if (!IsQuestActive())
            {
                return;
            }

            // 2. Kutu kontrolü
            if (PlayerCarryController.Instance == null || !PlayerCarryController.Instance.HasBox())
            {
                return;
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

            // Show the box in hand when an item is picked up
            if (PlayerCarryController.Instance != null)
            {
                PlayerCarryController.Instance.ShowBox();
            }

            if (collectEffect != null)
            {
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            }

            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }
            
            // Crucial: Clean up highlight before destroying
            OnHoverExit(); 
            Destroy(gameObject);
        }
    }
}
