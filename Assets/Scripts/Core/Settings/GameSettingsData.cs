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

    // Phím đổi vũ khí chính/phụ đang trang bị (không thuộc Hotbar)
    public KeyCode weaponSlot1 = KeyCode.Alpha1;
    public KeyCode weaponSlot2 = KeyCode.Alpha2;

    // Phím tắt dùng nhanh vật phẩm Hotbar (Consumable/Throwable), mặc định phím 3-7 vì 1-2 dành cho đổi vũ khí
    public KeyCode slot1 = KeyCode.Alpha3;
    public KeyCode slot2 = KeyCode.Alpha4;
    public KeyCode slot3 = KeyCode.Alpha5;
    public KeyCode slot4 = KeyCode.Alpha6;
    public KeyCode slot5 = KeyCode.Alpha7;
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

