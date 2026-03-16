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
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (visualBox != null)
            {
                visualBox.SetActive(false);
            }
        }

        public void ShowBox()
        {
            if (visualBox != null)
            {
                visualBox.SetActive(true);
            }
        }

        public void HideBox()
        {
            if (visualBox != null)
            {
                visualBox.SetActive(false);
            }
        }

        public bool HasBox()
        {
            return visualBox != null && visualBox.activeSelf;
        }
    }
}
