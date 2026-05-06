using QuestSystem;
using UnityEngine;

namespace QuestSystem
{
    public class Note : InteractableBase
    {
        private bool _hasBeenRead = false;

        public override void Interact()
        {
            ReadNote();
        }

        private void ReadNote()
        {
            if (NoteUI.Instance != null)
            {
                NoteUI.Instance.OpenNote(() =>
                {
                    if (!_hasBeenRead)
                    {
                        _hasBeenRead = true;
                        if (QuestManager.Instance != null)
                        {
                            QuestManager.Instance.CompleteCurrentQuest();
                        }
                    }
                });
            }
        }
    }
}
