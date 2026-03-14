using UnityEngine;
using QuestSystem;

namespace QuestSystem
{
    public class BoxPickup : InteractableBase
    {
        [Header("Quest Settings")]
        [SerializeField] private Quest boxQuest; // E.g., "Kutuyu Al" quest
        
        [Header("Effects")]
        [SerializeField] private GameObject pickupEffect;
        [SerializeField] private AudioClip pickupSound;

        public override void Interact()
        {
            Quest activeQuest = QuestManager.Instance.GetActiveQuest();

            // 1. Görev Sırası Kontrolü: Eğer bu kutu şu anki görev değilse alma.
            if (boxQuest != null && activeQuest != boxQuest)
            {
                return;
            }

            Pickup();
        }

        private void Pickup()
        {
            // 2. Eldeki kutuyu görünür yap (Sizin isteğinizle eldeki kutu aktif olacak)
            if (PlayerCarryController.Instance != null)
            {
                PlayerCarryController.Instance.ShowBox();
            }

            // 3. Görevi tamamla (QuestManager otomatik olarak sıradaki görevi açacak)
            QuestManager.Instance.CompleteCurrentQuest();

            // 4. Efekt ve Ses
            if (pickupEffect != null) Instantiate(pickupEffect, transform.position, Quaternion.identity);
            if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // 5. Yerden bu kutuyu SİL (Yerdeki kutu hemen yok olsun)
            OnHoverExit(); 
            Destroy(gameObject);
        }
    }
}
