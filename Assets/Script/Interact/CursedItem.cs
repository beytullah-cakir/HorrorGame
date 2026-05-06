using UnityEngine;

namespace QuestSystem
{
    public class CursedItem : InteractableBase
    {

        public override void Interact()
        {
            if (PlayerCarryController.Instance != null && PlayerCarryController.Instance.HasCursedItem())
            {
                // Zaten elimizde lanetli bir eşya var, başka alamayız
                Debug.Log("Zaten bir lanetli eşya taşıyorsun!");
                return;
            }

            Collect();
        }

        public override bool CanInteract()
        {
            // Sadece elimizde lanetli eşya yoksa etkileşime girebiliriz
            if (PlayerCarryController.Instance != null && PlayerCarryController.Instance.HasCursedItem())
            {
                return false;
            }
            return true;
        }

        private void Collect()
        {
            if (PlayerCarryController.Instance != null)
            {
                PlayerCarryController.Instance.PickUpCursedItem(this.gameObject);
            }

        }
    }
}
