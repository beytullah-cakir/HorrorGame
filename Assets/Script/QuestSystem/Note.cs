using QuestSystem;
using UnityEngine;

namespace QuestSystem
{
    public class Note : QuestItem
    {
        private bool _hasBeenRead = false;

        private void Start()
        {
            requiresBox = false;
            destroyOnCollect = false;
        }

        public override void Interact()
        {
            if (!IsQuestActive()) return;

            
            ReadNote();

            if (!_hasBeenRead)
            {
                _hasBeenRead = true;
                
                if (completeSubTask)
                {
                    QuestManager.Instance.CompleteSubTask(subTaskIndex);
                }
            }
        }
        private void ReadNote()
        {
            if (NoteUI.Instance != null)
            {
                NoteUI.Instance.OpenNote();
            }
            else
            {
                Debug.LogWarning("NoteUI instance bulunamadı!");
            }
        }
    }
}
