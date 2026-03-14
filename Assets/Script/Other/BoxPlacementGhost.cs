using UnityEngine;
using QuestSystem;

namespace QuestSystem
{
    public class BoxPlacementGhost : InteractableBase
    {
        [Header("Quest Activation")]
        [SerializeField] private Quest requiredQuest;
        

        [Header("Material Settings")]
        [Tooltip("Kutu yerleştirilince şeffaf kutunun dönüşeceği normal materyal.")]
        [SerializeField] private Material filledMaterial;
        
        private bool _isActivated = false;
        private bool _isPlaced = false;
        private Collider _collider;

        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<Collider>();
            
            // Oyun başında renderer ve collider kapalı olur (Raycast algılamaz)
            if (targetRenderer != null) targetRenderer.enabled = false;
            if (_collider != null) _collider.enabled = false;
        }

        private void OnEnable()
        {
            QuestManager.OnQuestCompleted += HandleQuestCompleted;
        }

        private void OnDisable()
        {
            QuestManager.OnQuestCompleted -= HandleQuestCompleted;
        }

        private void HandleQuestCompleted(Quest quest)
        {
            if (quest == requiredQuest && !_isPlaced)
            {
                _isActivated = true;
                if (targetRenderer != null) targetRenderer.enabled = true;
                if (_collider != null) _collider.enabled = true;
            }
        }

        public override void Interact()
        {
            if (!_isActivated || _isPlaced) return;

            if (PlayerCarryController.Instance != null && PlayerCarryController.Instance.HasBox())
            {
                PlaceBox();
            }
        }

        private void PlaceBox()
        {
            _isPlaced = true;
            _isActivated = false;

            // 1. Önce üzerindeki vurguyu (highlight) temizle ki eski materyali geri yüklemesin
            OnHoverExit();

            PlayerCarryController.Instance.HideBox();

            // 2. Materyali tamamen silip yenisini ekle
            if (targetRenderer != null && filledMaterial != null)
            {
                targetRenderer.enabled = true;
                Material[] newMats = new Material[] { filledMaterial };
                targetRenderer.materials = newMats;
                
                // 3. Base class'taki orijinal materyal listesini de güncelle (Güvenlik için)
                _originalMaterials = newMats;
            }

            this.enabled = false; 
        }
    }
}
