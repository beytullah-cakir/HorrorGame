using UnityEngine;
using QuestSystem;

namespace QuestSystem
{
    public class BoxPickup : InteractableBase
    {
        [Header("Quest Condition")]
        [Tooltip("Bu kutu sadece bu görev aktifken alınabilir. (Boş bırakılırsa her zaman alınabilir)")]
        [SerializeField] private Quest requiredQuest; 
        [SerializeField] private int subTaskIndex = 0;

        [Header("Effects")]
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
            QuestManager.Instance.CompleteCurrentQuest();

           
            if (PlayerCarryController.Instance != null)
            {
                PlayerCarryController.Instance.ShowBox(true);
            }

            
            if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            Destroy(gameObject);
        }
    }
}
