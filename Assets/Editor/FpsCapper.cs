using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using System.Linq;
using System.Threading;

namespace EditorUtils {

//
// Serializable settings
//
[FilePath("UserSettings/FpsCapperSettings.asset",
          FilePathAttribute.Location.ProjectFolder)]
public sealed class FpsCapperSettings : ScriptableSingleton<FpsCapperSettings>
{
    public bool enable = false;
    public int targetFrameRate = 60;
    public void Save() => Save(true);
    void OnDisable() => Save();
}

//
// Settings GUI
//
sealed class FpsCapperSettingsProvider : SettingsProvider
{
    public FpsCapperSettingsProvider()
      : base("Project/FPS Capper", SettingsScope.Project) {}

    public override void OnGUI(string search)
    {
        var settings = FpsCapperSettings.instance;
        var enable = settings.enable;
        var fps = settings.targetFrameRate;

        EditorGUI.BeginChangeCheck();

        enable = EditorGUILayout.Toggle("Enable", enable);
        fps = EditorGUILayout.IntField("Target Frame Rate", fps);

        if (EditorGUI.EndChangeCheck())
        {
            settings.enable = enable;
            settings.targetFrameRate = fps;
            settings.Save();
        }
    }

    [SettingsProvider]
    public static SettingsProvider CreateCustomSettingsProvider()
      => new FpsCapperSettingsProvider();
}

//
// Player loop system
//
[UnityEditor.InitializeOnLoad]
sealed class FpsCapperSystem
{
    // Synchronization object
    static AutoResetEvent _sync;

    // Interval in milliseconds
    static int IntervalMsec;

    // Interval thread function
    static void IntervalThread()
    {
        _sync = new AutoResetEvent(true);

        while (true)
        {
            Thread.Sleep(Mathf.Max(1, IntervalMsec));
            _sync.Set();
        }
    }

    // Custom system update function
    static void UpdateSystem()
    {
        var cfg = FpsCapperSettings.instance;

        // Property update
        IntervalMsec = 1000 / Mathf.Max(5, cfg.targetFrameRate);

        // Rejection cases
        if (_sync == null) return;              // Not ready
        if (!cfg.enable) return;                // Not enabled
        if (cfg.targetFrameRate < 1) return;    // Wrong FPS value
        if (Time.captureDeltaTime != 0) return; // Recording

        // Synchronization with the interval thread
        _sync.WaitOne();
    }

    // Static constructor (custom system installation)
    static FpsCapperSystem()
    {
        // Interval thread launch
        new Thread(IntervalThread).Start();

        // Custom system definition
        var system = new PlayerLoopSystem()
          { type = typeof(FpsCapperSystem),
            updateDelegate = UpdateSystem };

        // Custom system insertion
        var playerLoop = PlayerLoop.GetCurrentPlayerLoop();

        for (var i = 0; i < playerLoop.subSystemList.Length; i++)
        {
            ref var phase = ref playerLoop.subSystemList[i];
            if (phase.type == typeof(UnityEngine.PlayerLoop.EarlyUpdate))
            {
                phase.subSystemList
                  = phase.subSystemList.Concat(new[]{system}).ToArray();
                break;
            }
        }

        PlayerLoop.SetPlayerLoop(playerLoop);
    }
}

} // namespace EditorUtils
