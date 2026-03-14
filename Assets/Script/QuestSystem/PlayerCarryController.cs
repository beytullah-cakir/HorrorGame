using UnityEngine;

namespace QuestSystem
{
    public class PlayerCarryController : MonoBehaviour
    {
        public static PlayerCarryController Instance { get; private set; }

        [Header("Box Visual Settings")]
        [SerializeField] private GameObject visualBox; // Elindeki kutu objesi (Sahnede player altında duracak)

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            // Oyun başında eldeki kutu görünmez olsun
            if (visualBox != null) visualBox.SetActive(false);
        }

        public void ShowBox()
        {
            if (visualBox != null)
            {
                visualBox.SetActive(true);
                Debug.Log("[PlayerCarryController] Kutu ele alındı (Görünür).");
            }
        }

        public void HideBox()
        {
            if (visualBox != null)
            {
                visualBox.SetActive(false);
                Debug.Log("[PlayerCarryController] Kutu yerleştirildi (Gizli).");
            }
        }

        public bool HasBox()
        {
            return visualBox != null && visualBox.activeSelf;
        }
    }
}
