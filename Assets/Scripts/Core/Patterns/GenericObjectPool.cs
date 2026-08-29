using System;
using UnityEngine;

namespace PROJ.Patterns
{
    /// <summary>
    /// Generic Object Pool Pattern độc lập, dùng để quản lý tái sử dụng các object (Đạn, Hiệu ứng VFX, Item...) 
    /// nhằm tránh chi phí Instantiate/Destroy liên tục gây rác Garbage Collection.
    /// </summary>
    public class GenericObjectPool<T> where T : Component
    {
        private readonly T prefab;
        private readonly Transform parentTransform;
        private readonly System.Collections.Generic.Queue<T> poolQueue = new();

        public GenericObjectPool(T prefab, int initialCapacity = 10, Transform parentTransform = null)
        {
            this.prefab = prefab;
            this.parentTransform = parentTransform;

            for (int i = 0; i < initialCapacity; i++)
            {
                T obj = GameObject.Instantiate(prefab, parentTransform);
                obj.gameObject.SetActive(false);
                poolQueue.Enqueue(obj);
            }
        }

        public T Get(Vector3 position, Quaternion rotation)
        {
            T obj;
            if (poolQueue.Count > 0)
            {
                obj = poolQueue.Dequeue();
                if (obj == null)
                {
                    obj = GameObject.Instantiate(prefab, parentTransform);
                }
            }
            else
            {
                obj = GameObject.Instantiate(prefab, parentTransform);
            }

            Transform objTransform = obj.transform;
            objTransform.SetPositionAndRotation(position, rotation);
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void ReturnToPool(T obj)
        {
            if (obj == null) return;
            obj.gameObject.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }
}
