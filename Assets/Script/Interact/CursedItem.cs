using UnityEngine;

namespace QuestSystem
{
    public class CursedItem : QuestItem
    {
        private void Awake()
        {
            // Lanetli eşyalar kutu gerektirmez ve yerden alındığında hemen yok olmaz (oyuncuya bağlanır)
            requiresBox = false;
            destroyOnCollect = false;
        }

        public override void Interact()
        {
            if (PlayerCarryController.Instance != null && PlayerCarryController.Instance.HasCursedItem())
            {
                Debug.Log("Zaten bir lanetli eşya taşıyorsun!");
                return;
            }

            base.Interact();
        }

        public override bool CanInteract()
        {
            // Elimizde zaten bir eşya varsa etkileşime giremeyiz
            if (PlayerCarryController.Instance != null && PlayerCarryController.Instance.HasCursedItem())
            {
                return false;
            }

            return base.CanInteract();
        }

        public override void Collect()
        {
            // Yerden alırken görev sistemini tetikleme! 
            // Görev sadece ateşe atınca ilerleyecek.

            // Eşyayı oyuncunun eline ver ve bu eşyanın subtask bilgisini PlayerCarryController'a kaydet
            if (PlayerCarryController.Instance != null)
            {
                PlayerCarryController.Instance.PickUpCursedItem(this.gameObject, GetSubTaskIndex());
            }
        }
    }
}
