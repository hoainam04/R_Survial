using System;
using System.IO;
using UnityEngine;

public static class JsonStorage
{
    private static string BasePath => Application.persistentDataPath;

    // Lớp bọc Generic để giúp JsonUtility lưu được các kiểu dữ liệu đơn lẻ hoặc List
    [Serializable]
    private class Wrapper<T>
    {
        public T value;
    }

    /// <summary>
    /// Saves any data object into a JSON file using Unity's native JsonUtility.
    /// </summary>
    public static void Save<T>(string fileName, T data)
    {
        try
        {
            if (!fileName.EndsWith(".json")) fileName += ".json";
            string fullPath = Path.Combine(BasePath, fileName);

            string jsonString = "";

            // Kiểm tra nếu là Class thông thường thì chuyển đổi trực tiếp
            // Nếu là kiểu nguyên thủy (int, string...) hoặc List thì phải bọc lại
            if (typeof(T).IsClass && !typeof(T).IsGenericType && typeof(T) != typeof(string))
            {
                jsonString = JsonUtility.ToJson(data, true);
            }
            else
            {
                Wrapper<T> wrapper = new Wrapper<T> { value = data };
                jsonString = JsonUtility.ToJson(wrapper, true);
            }

            File.WriteAllText(fullPath, jsonString);
            Debug.Log($"<color=green>[JsonStorage]</color> Successfully saved to: {fullPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[JsonStorage] Failed to save {fileName}: {e.Message}");
        }
    }

    /// <summary>
    /// Loads data from a JSON file into the specified type using Unity's native JsonUtility.
    /// </summary>
    public static T Load<T>(string fileName, T defaultValue = default)
    {
        try
        {
            if (!fileName.EndsWith(".json")) fileName += ".json";
            string fullPath = Path.Combine(BasePath, fileName);

            if (!File.Exists(fullPath))
            {
                Debug.LogWarning($"[JsonStorage] File not found: {fileName}. Returning default value.");
                return defaultValue;
            }

            string jsonString = File.ReadAllText(fullPath);

            // Xử lý ngược lại khi đọc file
            if (typeof(T).IsClass && !typeof(T).IsGenericType && typeof(T) != typeof(string))
            {
                return JsonUtility.FromJson<T>(jsonString);
            }
            else
            {
                Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(jsonString);
                return wrapper != null ? wrapper.value : defaultValue;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[JsonStorage] Failed to load {fileName}: {e.Message}");
            return defaultValue;
        }
    }
    public static void GetFullPath(string fileName, out string fullPath)
    {
        if (!fileName.EndsWith(".json")) fileName += ".json";
        fullPath = Path.Combine(BasePath, fileName);
    }
}