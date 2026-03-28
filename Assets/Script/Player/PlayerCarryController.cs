using UnityEngine;
using System.Collections.Generic;

namespace QuestSystem
{
    public class PlayerCarryController : MonoBehaviour
    {
        public static PlayerCarryController Instance { get; private set; }

        [Header("Box Visual Settings")]
        [SerializeField] private GameObject visualBox;

        private void Awake()
        {
            Instance = this;
            visualBox.SetActive(false);
        }

        public void ShowBox(bool show)=> visualBox.SetActive(show);

        public bool HasBox() { return visualBox != null && visualBox.activeSelf; }
        
    }
}
