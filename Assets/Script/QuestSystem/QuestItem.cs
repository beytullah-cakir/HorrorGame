using UnityEngine;

namespace QuestSystem
{
    public class QuestItem : InteractableBase
    {
        [Header("Collection Settings")]
        [SerializeField] private bool completeSubTask = true;
        [SerializeField] private int subTaskIndex = 0;
        [SerializeField] private bool interactToCollect = true;
        
        [Header("Effects")]
        [SerializeField] private GameObject collectEffect;
        [SerializeField] private AudioClip collectSound;

        protected override void Awake()
        {
            base.Awake();
        }
        public override void Interact()
        {
            if (interactToCollect)
            {
                Collect();
            }
        }

        public void Collect()
        {
            if (completeSubTask)
            {
                QuestManager.Instance.CompleteSubTask(subTaskIndex);
            }
            else
            {
                QuestManager.Instance.CompleteCurrentQuest();
            }

            if (collectEffect != null)
            {
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            }

            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            Debug.Log($"[QuestItem] Item {gameObject.name} collected!");
            
            // Crucial: Clean up highlight before destroying
            OnHoverExit(); 
            Destroy(gameObject);
        }
    }
}
