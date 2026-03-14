using UnityEngine;
using System.Collections.Generic;

namespace QuestSystem
{
    public class PlayerCarryController : MonoBehaviour
    {
        public static PlayerCarryController Instance { get; private set; }

        [Header("Box Visual Settings")]
        [SerializeField] private GameObject visualBox; 

        [Header("Animation Settings")]
        [SerializeField] private Animator carriedBoxAnimator;
        [SerializeField] private string closeTriggerName = "Close";

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (visualBox != null) visualBox.SetActive(false);
        }

        private void OnEnable()
        {
            QuestManager.OnQuestCompleted += HandleQuestCompleted;
        }

        private void OnDisable()
        {
            QuestManager.OnQuestCompleted -= HandleQuestCompleted;
        }

        private bool _skipFirstCompletion = false; 

        private void HandleQuestCompleted(Quest quest)
        {
            if (!HasBox())
            {
                return;
            }

            if (_skipFirstCompletion)
                {
                    _skipFirstCompletion = false;
                    return;
                }

            if (carriedBoxAnimator != null)
            {
                carriedBoxAnimator.SetTrigger(closeTriggerName);
            }
        }

        public void ShowBox()
        {
            if (visualBox != null)
            {
                if (!visualBox.activeSelf)
                {
                    _skipFirstCompletion = true; 
                }
                visualBox.SetActive(true);
            }
        }

        public void HideBox()
        {
            if (visualBox != null)
            {
                visualBox.SetActive(false);

                // Kutu yerleştirilince animator'ı sıfırla ki bir sonraki alışta açık görünsün.
                if (carriedBoxAnimator != null)
                {
                    carriedBoxAnimator.Rebind();
                }
            }
        }

        public bool HasBox()
        {
            return visualBox != null && visualBox.activeSelf;
        }
    }
}
