using System;
using System.Collections.Generic;
using UnityEngine;

namespace PROJ.Patterns
{
    // Interface để các object tự reset trạng thái khi lấy ra / thu hồi
    public interface IPoolable
    {
        void OnSpawnFromPool();
        void OnReturnToPool();
    }

    public class GenericObjectPool<T> where T : Component
    {
        private readonly T prefab;
        private readonly Transform parentTransform;
        private readonly Queue<T> poolQueue;
        private readonly int maxSize;

        public GenericObjectPool(T prefab, int initialCapacity = 10, int maxSize = 100, Transform parentTransform = null)
        {
            this.prefab = prefab;
            this.parentTransform = parentTransform;
            this.maxSize = maxSize;
            this.poolQueue = new Queue<T>(initialCapacity);

            for (int i = 0; i < initialCapacity; i++)
            {
                CreateNewObject();
            }
        }

        private T CreateNewObject()
        {
            T obj = GameObject.Instantiate(prefab, parentTransform);
            obj.gameObject.SetActive(false);
            poolQueue.Enqueue(obj);
            return obj;
        }

        public T Get(Vector3 position, Quaternion rotation)
        {
            T obj = null;

            while (poolQueue.Count > 0 && obj == null)
            {
                obj = poolQueue.Dequeue();
            }

            if (obj == null)
            {
                obj = GameObject.Instantiate(prefab, parentTransform);
            }

            Transform objTransform = obj.transform;
            objTransform.SetPositionAndRotation(position, rotation);
            obj.gameObject.SetActive(true);

            // Tự động kích hoạt logic reset nếu component kế thừa IPoolable
            if (obj is IPoolable poolable)
            {
                poolable.OnSpawnFromPool();
            }

            return obj;
        }

        public void ReturnToPool(T obj)
        {
            if (obj == null) return;

            // Chặn bug trả về pool nhiều lần
            if (!obj.gameObject.activeSelf) return;

            if (obj is IPoolable poolable)
            {
                poolable.OnReturnToPool();
            }

            obj.gameObject.SetActive(false);

            // Nếu vượt quá giới hạn tối đa thì hủy bớt để giải phóng RAM
            if (poolQueue.Count >= maxSize)
            {
                GameObject.Destroy(obj.gameObject);
                return;
            }

            poolQueue.Enqueue(obj);
        }
    }
}