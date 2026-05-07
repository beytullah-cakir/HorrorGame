using UnityEngine;

namespace QuestSystem
{
    public class SaltItem : InteractableBase
    {
        [Header("Quest Settings")]
        [SerializeField] private Quest requiredQuest;

        [Header("Sound Settings")]
        [SerializeField] private AudioSource pourAudioSource;

        // Sesin doğrudan çalınması için
        public void PlayPourSoundEvent()
        {
            if (pourAudioSource != null)
            {
                pourAudioSource.Play();
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
