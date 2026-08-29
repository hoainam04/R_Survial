using UnityEngine;

public class GameSettingsManager : MonoBehaviour
    {
        private const string SETTINGS_FILENAME = "gamesettings.json";

        public static GameSettingsManager Instance { get; private set; }

        [SerializeField] private KeybindSettings keybinds = new();
        [SerializeField] private AudioSettings audio = new();
        [SerializeField] private GraphicsSettings graphics = new();

        public KeybindSettings Keybinds => keybinds;
        public AudioSettings Audio => audio;
        public GraphicsSettings Graphics => graphics;

        public event System.Action OnSettingsChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadSettings();
            ApplySettings();
        }

        public void SaveSettings()
        {
            var data = new GameSettingsContainer
            {
                keybinds = this.keybinds,
                audio = this.audio,
                graphics = this.graphics
            };

            JsonStorage.Save(SETTINGS_FILENAME, data);
            Debug.Log("<color=cyan>[GameSettingsManager]</color> Settings saved successfully.");
            OnSettingsChanged?.Invoke();
        }

        public void LoadSettings()
        {
            var data = JsonStorage.Load<GameSettingsContainer>(SETTINGS_FILENAME);
            if (data != null)
            {
                this.keybinds = data.keybinds ?? new KeybindSettings();
                this.audio = data.audio ?? new AudioSettings();
                this.graphics = data.graphics ?? new GraphicsSettings();
                Debug.Log("<color=cyan>[GameSettingsManager]</color> Settings loaded successfully.");
                JsonStorage.GetFullPath(SETTINGS_FILENAME, out string fullPath);
                Debug.Log($"<color=cyan>[GameSettingsManager]</color> Settings loaded from: {fullPath}");
            }
            else
            {
                Debug.LogWarning("[GameSettingsManager] No saved settings found. Using defaults.");
                ResetToDefaults();
            }
        }

        public void ResetToDefaults()
        {
            keybinds = new KeybindSettings();
            audio = new AudioSettings();
            graphics = new GraphicsSettings();
            SaveSettings();
            ApplySettings();
        }

        public void ApplySettings()
        {
            // Apply framerate
            Application.targetFrameRate = graphics.targetFramerate;

            // Apply screen mode
            Screen.fullScreen = graphics.fullscreen;

            // Apply quality
            QualitySettings.SetQualityLevel(graphics.qualityLevel);

            // Apply audio (if AudioListener exists)
            AudioListener.volume = audio.masterVolume;
        }

        // Helper check keybind method
        public bool GetKeyDown(string actionName)
        {
            KeyCode code = GetKeycodeForAction(actionName);
            return code != KeyCode.None && Input.GetKeyDown(code);
        }

        public bool GetKey(string actionName)
        {
            KeyCode code = GetKeycodeForAction(actionName);
            return code != KeyCode.None && Input.GetKey(code);
        }

        public bool GetKeyUp(string actionName)
        {
            KeyCode code = GetKeycodeForAction(actionName);
            return code != KeyCode.None && Input.GetKeyUp(code);
        }

        private KeyCode GetKeycodeForAction(string actionName)
        {
            return actionName switch
            {
                "Sprint" => keybinds.sprint,
                "Jump" => keybinds.jump,
                "Dash" => keybinds.dash,
                "Attack" => keybinds.attack,
                "Reload" => keybinds.reload,
                "Interact" => keybinds.interact,
                "Inventory" => keybinds.inventory,
                "Pause" => keybinds.pause,
                "Slot1" => keybinds.slot1,
                "Slot2" => keybinds.slot2,
                "Slot3" => keybinds.slot3,
                "Slot4" => keybinds.slot4,
                "Slot5" => keybinds.slot5,
                _ => KeyCode.None
            };
        }
    }

    [System.Serializable]
    public class GameSettingsContainer
    {
        public KeybindSettings keybinds;
        public AudioSettings audio;
        public GraphicsSettings graphics;
    }
