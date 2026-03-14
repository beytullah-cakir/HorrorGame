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

        public override void Interact()
        {
            Quest activeQuest = QuestManager.Instance.GetActiveQuest();
            
            // 1. Görev sırası kontrolü
            if (requiredQuest != null && activeQuest != requiredQuest)
            {
                Debug.Log($"[QuestItem] Henüz bu göreve gelmediniz! Aktif görev: {(activeQuest != null ? activeQuest.title : "Yok")}");
                return;
            }

            // 2. Kutu kontrolü (Artık her küçük eşya için elinde kutu olması ŞART)
            if (PlayerCarryController.Instance == null || !PlayerCarryController.Instance.HasBox())
            {
                Debug.Log("[QuestItem] Eşyaları toplamak için önce elinizde bir kutu olmalı!");
                return;
            }

            if (interactToCollect)
            {
                Collect();
            }
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

            Debug.Log($"[QuestItem] Item {gameObject.name} collected!");
            
            // Crucial: Clean up highlight before destroying
            OnHoverExit(); 
            Destroy(gameObject);
        }
    }
}
