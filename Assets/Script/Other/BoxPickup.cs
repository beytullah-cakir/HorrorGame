using UnityEngine;
using QuestSystem;

namespace QuestSystem
{
    public class BoxPickup : InteractableBase
    {
        [Header("Quest Condition")]
        [Tooltip("Bu kutu sadece bu görev aktifken alınabilir. (Boş bırakılırsa her zaman alınabilir)")]
        [SerializeField] private Quest requiredQuest; 
        
        [Header("Effects")]
        [SerializeField] private GameObject pickupEffect;
        [SerializeField] private AudioClip pickupSound;

        public override void Interact()
        {
            // Eğer bir görev gereksinimi varsa ve o görev aktif değilse kutuyu alma.
            if (requiredQuest != null && QuestManager.Instance.GetActiveQuest() != requiredQuest)
            {
                return;
            }

            Pickup();
        }

        private void Pickup()
        {
            // Eldeki kutuyu görünür yap
            if (PlayerCarryController.Instance != null)
            {
                PlayerCarryController.Instance.ShowBox();
            }

            // Efekt ve Ses
            if (pickupEffect != null) Instantiate(pickupEffect, transform.position, Quaternion.identity);
            if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // Yerden bu kutuyu SİL
            OnHoverExit(); 
            Destroy(gameObject);
        }
    }
}
