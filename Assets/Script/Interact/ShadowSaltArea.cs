using UnityEngine;
using UnityEngine.Events;

namespace QuestSystem
{
    public class ShadowSaltArea : InteractableBase
    {
        [Header("Quest Settings")]
        [SerializeField] private Quest requiredQuest;
        [SerializeField] private bool completeSubTaskOnPour = true;
        [SerializeField] private int subTaskIndex = 0;

        [Header("Salt Settings")]
        [SerializeField] private bool requiresSaltInInventory = true;
        

        [Header("Events")]
        public UnityEvent onSaltPoured;

        private MeshRenderer _meshRenderer;
        private bool _isSaltPoured = false;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            if (_meshRenderer != null)
            {
                _meshRenderer.enabled = false;
            }
        }

        private void Update()
        {
            if (_isSaltPoured) return;

            if (_meshRenderer != null)
            {
                // Görev aktifse mesh renderer'ı aç
                bool shouldBeVisible = IsQuestActive();
                if (_meshRenderer.enabled != shouldBeVisible)
                {
                    _meshRenderer.enabled = shouldBeVisible;
                }
            }
        }

        public override void Interact()
        {
            if (!IsQuestActive() || _isSaltPoured)
            {
                return;
            }

            if (requiresSaltInInventory)
            {
                if (PlayerCarryController.Instance == null || !PlayerCarryController.Instance.HasSalt())
                {
                    Debug.Log("Tuz dökebilmek için elinde tuz olması gerekiyor!");
                    return;
                }
            }

            PourSalt();
        }

        public override bool CanInteract()
        {
            if (_isSaltPoured) return false;
            
            bool hasRequiredItem = !requiresSaltInInventory || (PlayerCarryController.Instance != null && PlayerCarryController.Instance.HasSalt());
            return IsQuestActive() && hasRequiredItem;
        }

        private bool IsQuestActive()
        {
            Quest activeQuest = QuestManager.Instance.GetActiveQuest();
            return requiredQuest == null || activeQuest == requiredQuest;
        }

        private void PourSalt()
        {
            _isSaltPoured = true;
            // Görev tamamlama
            if (completeSubTaskOnPour && QuestManager.Instance != null)
            {
                QuestManager.Instance.CompleteSubTask(subTaskIndex);
            }

            onSaltPoured?.Invoke();
            
            // Tuz serpildikten sonra gölge alanı inaktif yapılır
            if (_meshRenderer != null) _meshRenderer.enabled = false;
            
            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }
        }
    }
}
