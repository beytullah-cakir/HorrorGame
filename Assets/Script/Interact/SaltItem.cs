using UnityEngine;

namespace QuestSystem
{
    public class SaltItem : InteractableBase
    {
        [Header("Quest Settings")]
        [SerializeField] private Quest requiredQuest;

        [Header("Animation & Sound")]
        [SerializeField] private Animator itemAnimator;
        [SerializeField] private AudioClip pourSound;

        // Bu metod, tuz dökme animasyonunun içine koyacağınız Animation Event tarafından çağrılacak
        public void PlayPourSoundEvent()
        {
            if (pourSound != null)
            {
                AudioSource.PlayClipAtPoint(pourSound, transform.position);
            }
        }

        // Bu metod, tuz dökme animasyonu bittiğinde çağrılacak Animation Event
        public void FinishPourAnimationEvent()
        {
            FirstPersonController player = FindObjectOfType<FirstPersonController>();
            if (player != null)
            {
                player.UnlockPlayerAll();
            }
        }

        public void TriggerPourAnimation()
        {
            if (itemAnimator != null)
            {
                itemAnimator.SetTrigger("PourSalt"); // Animator'daki tetikleyici adı
            }

            FirstPersonController player = FindObjectOfType<FirstPersonController>();
            if (player != null)
            {
                player.LockPlayerAll();
            }
        }

        public override void Interact()
        {
            if (!IsQuestActive())
            {
                return;
            }

            if (PlayerCarryController.Instance != null && PlayerCarryController.Instance.HasSalt())
            {
                Debug.Log("Zaten elinde tuz var!");
                return;
            }

            Collect();
        }

        public override bool CanInteract()
        {
            if (!IsQuestActive()) return false;

            if (PlayerCarryController.Instance != null && PlayerCarryController.Instance.HasSalt())
            {
                return false;
            }
            return true;
        }

        private bool IsQuestActive()
        {
            Quest activeQuest = QuestManager.Instance.GetActiveQuest();
            return requiredQuest == null || activeQuest == requiredQuest;
        }

        private void Collect()
        {
            if (PlayerCarryController.Instance != null)
            {
                PlayerCarryController.Instance.PickUpSaltItem(this.gameObject);
            }
        }
    }
}
