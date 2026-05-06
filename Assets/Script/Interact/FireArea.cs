using UnityEngine;

namespace QuestSystem
{
    public class FireArea : InteractableBase
    {
        [Header("Settings")]
        [SerializeField] private bool completeSubTaskOnBurn = true;
        [SerializeField] private int subTaskIndex = 0;

        [Header("Effects")]
        [SerializeField] private GameObject burnEffect;
        [SerializeField] private AudioClip burnSound;

        public override void Interact()
        {
            if (PlayerCarryController.Instance != null && PlayerCarryController.Instance.HasCursedItem())
            {
                BurnCursedItem();
            }
        }

        public override bool CanInteract()
        {
            // Sadece elimizde lanetli eşya varsa etkileşime girebiliriz
            if (PlayerCarryController.Instance != null && PlayerCarryController.Instance.HasCursedItem())
            {
                return true;
            }
            return false;
        }

        private void BurnCursedItem()
        {
            // Yakacağımız eşyanın hafızadaki subtask indeksini al
            int indexToComplete = subTaskIndex; // Varsayılan olarak Inspector'daki değer
            if (PlayerCarryController.Instance != null)
            {
                int savedIndex = PlayerCarryController.Instance.CurrentCursedItemSubTaskIndex;
                if (savedIndex >= 0)
                {
                    indexToComplete = savedIndex;
                }
                
                // Lanetli eşyayı elimizden al
                PlayerCarryController.Instance.DestroyCursedItem();
            }

            // Görev alt görevini tamamla
            if (completeSubTaskOnBurn && QuestManager.Instance != null)
            {
                Debug.Log($"[FireArea] Ateşe atıldı. Tamamlanan Subtask Index: {indexToComplete}");
                QuestManager.Instance.CompleteSubTask(indexToComplete);
            }

            // Efektleri oynat
            if (burnEffect != null)
            {
                Instantiate(burnEffect, transform.position, Quaternion.identity);
            }

            if (burnSound != null)
            {
                AudioSource.PlayClipAtPoint(burnSound, transform.position);
            }
        }
    }
}
