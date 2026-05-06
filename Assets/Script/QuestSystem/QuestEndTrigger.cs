using UnityEngine;

namespace QuestSystem
{
    public class QuestEndTrigger : MonoBehaviour
    {
        [Header("Activation Settings")]
        [Tooltip("Tüm görevler bittiğinde aktif edilecek obje.")]
        [SerializeField] private GameObject objectToActivate;

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
            // Eğer biten görev son görevse hedef objeyi aktif et
            if (QuestManager.Instance != null && QuestManager.Instance.IsLastQuest())
            {
                if (objectToActivate != null)
                {
                    objectToActivate.SetActive(true);
                    Debug.Log("Tüm görevler tamamlandı, obje aktif edildi: " + objectToActivate.name);
                }
            }
        }
    }
}
