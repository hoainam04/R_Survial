using System;
using UnityEngine;

namespace PROJ.Patterns
{
    /// <summary>
    /// Event Channel Pattern (Observer) độc lập dùng để truyền tải sự kiện toàn cục giữa các hệ thống (Gameplay, UI, Audio...)
    /// mà không tạo sự ràng buộc trực tiếp (Decoupled communication).
    /// </summary>
    [CreateAssetMenu(fileName = "NewGameEventChannel", menuName = "PROJ/Patterns/Event Channel")]
    public class GameEventChannel : ScriptableObject
    {
        private event Action<object> OnEventRaised;

        public void Raise(object value = null)
        {
            OnEventRaised?.Invoke(value);
        }

        public void RegisterListener(Action<object> listener)
        {
            OnEventRaised += listener;
        }

        public void UnregisterListener(Action<object> listener)
        {
            OnEventRaised -= listener;
        }
    }
}
