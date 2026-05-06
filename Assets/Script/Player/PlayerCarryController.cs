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
        
        [Header("Item Hold Settings")]
        [SerializeField] private Transform itemHoldPoint;

        private GameObject _currentCursedItem;
        private GameObject _currentSaltItem;

        public int CurrentCursedItemSubTaskIndex { get; private set; } = -1;

        public void PickUpCursedItem(GameObject itemObj, int subTaskIndex)
        {
            _currentCursedItem = itemObj;
            CurrentCursedItemSubTaskIndex = subTaskIndex;
            AttachToHoldPoint(itemObj);
        }

        public bool HasCursedItem()
        {
            return _currentCursedItem != null;
        }

        public void DestroyCursedItem()
        {
            if (_currentCursedItem != null)
            {
                Destroy(_currentCursedItem);
                _currentCursedItem = null;
            }
        }

        public void PickUpSaltItem(GameObject itemObj)
        {
            _currentSaltItem = itemObj;
            AttachToHoldPoint(itemObj);
        }

        public bool HasSalt()
        {
            return _currentSaltItem != null;
        }

        public void DestroySaltItem()
        {
            if (_currentSaltItem != null)
            {
                Destroy(_currentSaltItem);
                _currentSaltItem = null;
            }
        }

        public SaltItem GetCurrentSaltItem()
        {
            if (_currentSaltItem != null)
            {
                return _currentSaltItem.GetComponent<SaltItem>();
            }
            return null;
        }

        private void AttachToHoldPoint(GameObject itemObj)
        {
            if (itemHoldPoint == null) return;

            Collider col = itemObj.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            Rigidbody rb = itemObj.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            itemObj.transform.SetParent(itemHoldPoint);
            itemObj.transform.localPosition = Vector3.zero;
            itemObj.transform.localRotation = Quaternion.identity;
        }
    }
}
