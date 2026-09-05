using UnityEngine;

namespace PROJ.UI
{
    /// <summary>
    /// Generic controller for a vertical ScrollView.
    /// Responsible only for managing the content area and spawned list items.
    /// </summary>
    public class VerticalScrollList : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform content;

        [Header("Settings")]
        [SerializeField] private bool clearOnInitialize = true;

        public RectTransform Content => content;

        private void Awake()
        {
            if (clearOnInitialize)
                Clear();
        }

        /// <summary>
        /// Spawn an item under the ScrollView content.
        /// </summary>
        public T SpawnItem<T>(T prefab) where T : Component
        {
            if (prefab == null)
            {
                Debug.LogError($"[{nameof(VerticalScrollList)}] Prefab is null.", this);
                return null;
            }

            if (content == null)
            {
                Debug.LogError($"[{nameof(VerticalScrollList)}] Content is not assigned.", this);
                return null;
            }

            return Instantiate(prefab, content);
        }

        /// <summary>
        /// Spawn a GameObject under the ScrollView content.
        /// </summary>
        public GameObject SpawnItem(GameObject prefab)
        {
            if (prefab == null)
            {
                Debug.LogError($"[{nameof(VerticalScrollList)}] Prefab is null.", this);
                return null;
            }

            if (content == null)
            {
                Debug.LogError($"[{nameof(VerticalScrollList)}] Content is not assigned.", this);
                return null;
            }

            return Instantiate(prefab, content);
        }

        /// <summary>
        /// Remove all spawned items.
        /// </summary>
        public void Clear()
        {
            if (content == null)
                return;

            for (int i = content.childCount - 1; i >= 0; i--)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Returns the number of spawned items.
        /// </summary>
        public int ItemCount
        {
            get
            {
                return content != null ? content.childCount : 0;
            }
        }
    }
}