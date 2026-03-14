using UnityEngine;
using QuestSystem;

namespace QuestSystem
{
    public class BoxPlacementGhost : InteractableBase
    {
        [Header("Quest Activation")]
        [SerializeField] private Quest requiredQuest;
        [SerializeField] private bool activateOnStart = false;

        [Header("Material Settings")]
        [Tooltip("Kutu yerleştirilince şeffaf kutunun dönüşeceği normal materyal.")]
        [SerializeField] private Material filledMaterial;
        
        private bool _isActivated = false;
        private bool _isPlaced = false;
        private MeshRenderer _meshRenderer;

        protected override void Awake()
        {
            base.Awake();
            _meshRenderer = GetComponent<MeshRenderer>();
            
            // Oyun başında şeffaf kutu görünmez
            if (!activateOnStart) gameObject.SetActive(false);
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
                gameObject.SetActive(true);
                Debug.Log($"[BoxPlacementGhost] {gameObject.name} aktif oldu, kutu yerleştirilebilir.");
            }
        }

        public override void Interact()
        {
            if (!_isActivated || _isPlaced) return;

            // Oyuncunun elinde kutu var mı kontrolü
            if (PlayerCarryController.Instance != null && PlayerCarryController.Instance.HasBox())
            {
                PlaceBox();
            }
            else
            {
                Debug.Log("Elinizde yerleştirilecek bir kutu yok!");
            }
        }

        private void PlaceBox()
        {
            _isPlaced = true;
            _isActivated = false;

            // 1. Elindeki kutuyu gizle
            PlayerCarryController.Instance.HideBox();

            // 2. Şeffaf kutunun materyalini normal (dolu) materyale çevir
            if (_meshRenderer != null && filledMaterial != null)
            {
                _meshRenderer.material = filledMaterial;
            }

            // 3. Etkileşimi kapat (Artık üzerine gelinince parlama yapmasın)
            this.enabled = false; 

            Debug.Log($"[BoxPlacementGhost] Kutu yerleştirildi, materyal güncellendi.");
        }
    }
}
