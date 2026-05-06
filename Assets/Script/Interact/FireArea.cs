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
            // Lanetli eşyayı elimizden al
            if (PlayerCarryController.Instance != null)
            {
                PlayerCarryController.Instance.DestroyCursedItem();
            }

            // Görev alt görevini tamamla (isteğe bağlı)
            if (completeSubTaskOnBurn && QuestManager.Instance != null)
            {
                QuestManager.Instance.CompleteSubTask(subTaskIndex);
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
