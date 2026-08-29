using System;
using UnityEngine;


[Serializable]
public class KeybindSettings
{
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode sprint = KeyCode.LeftShift;
    public KeyCode jump = KeyCode.Space;
    public KeyCode dash = KeyCode.Space; // Có thể tùy chỉnh riêng hoặc dùng chung
    public KeyCode attack = KeyCode.Mouse0;
    public KeyCode reload = KeyCode.R;
    public KeyCode interact = KeyCode.F;
    public KeyCode inventory = KeyCode.I;
    public KeyCode pause = KeyCode.Escape;

    // Phím tắt chọn nhanh slot / item (1 đến 5)
    public KeyCode slot1 = KeyCode.Alpha1;
    public KeyCode slot2 = KeyCode.Alpha2;
    public KeyCode slot3 = KeyCode.Alpha3;
    public KeyCode slot4 = KeyCode.Alpha4;
    public KeyCode slot5 = KeyCode.Alpha5;
}

[Serializable]
public class AudioSettings
{
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
}

[Serializable]
public class GraphicsSettings
{
    public int qualityLevel = 2;
    public bool fullscreen = true;
    public int targetFramerate = 60;
}

