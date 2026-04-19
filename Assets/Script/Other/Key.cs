using UnityEngine;
using QuestSystem;

namespace QuestSystem
{
    public class Key : QuestItem
    {
        public bool hasKey;

        public static Key Instance;

        private void Awake()
        {
            Instance = this;
        }

        public override void Interact()
        {
            base.Interact();
            hasKey = true;

        }
    }
}
